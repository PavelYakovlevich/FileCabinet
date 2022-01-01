using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using FileCabinetApp.Utils;

namespace FileCabinetApp.Serialization
{
    /// <summary>
    ///     Class for working with objects, which are saved in memory.
    /// </summary>
    public partial class DumpHelper
    {
        private readonly Type objectType;
        private readonly BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        private readonly Dictionary<Type, Func<object, byte[]>> getBytesConverters = new Dictionary<Type, Func<object, byte[]>>()
        {
            { typeof(string), (str) => Encoding.Default.GetBytes((string)str) },
            { typeof(int), (integer) => BitConverter.GetBytes((int)integer) },
            { typeof(short), (integer) => BitConverter.GetBytes((short)integer) },
            { typeof(char), (ch) => BitConverter.GetBytes((char)ch) },
            { typeof(decimal), (number) => BitConverterExtension.GetBytes((decimal)number) },
            {
                typeof(DateTime), (date) =>
                {
                    var dateTime = (DateTime)date;
                    var result = new List<byte>(sizeof(int) * 3);
                    result.AddRange(BitConverter.GetBytes(dateTime.Year));
                    result.AddRange(BitConverter.GetBytes(dateTime.Month));
                    result.AddRange(BitConverter.GetBytes(dateTime.Day));
                    return result.ToArray();
                }
            },
            { typeof(byte[]), (bytes) => (byte[])bytes },
        };

        private readonly Dictionary<Type, Func<byte[], int, int, object>> toValueConverters = new Dictionary<Type, Func<byte[], int, int, object>>()
        {
            { typeof(string), (bytes, offset, length) => Encoding.Default.GetString(bytes, offset, length) },
            { typeof(int), (bytes, offset, lengthr) => BitConverter.ToInt32(bytes, offset) },
            { typeof(short), (bytes, offset, length) => BitConverter.ToInt16(bytes, offset) },
            { typeof(char), (bytes, offset, length) => BitConverter.ToChar(bytes, offset) },
            { typeof(decimal), (bytes, offset, length) => BitConverterExtension.ToDecimal(bytes[offset.. (offset + length)]) },
            {
                typeof(DateTime), (bytes, offset, length) =>
                {
                    var year = BitConverter.ToInt32(bytes, offset);
                    var month = BitConverter.ToInt32(bytes, offset + sizeof(int));
                    var day = BitConverter.ToInt32(bytes, offset + (sizeof(int) * 2));
                    return new DateTime(year, month, day);
                }
            },
            { typeof(byte[]), (bytes, offset, length) => (byte[])bytes },
        };

        private int precedingAreaSize = 0;
        private SortedList<int, DumpSliceMemberInfo> dumpMembersInfo;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DumpHelper"/> class.
        /// </summary>
        /// <param name="objectType"><see cref="Type"/> object, which represents type of manipulated objects.</param>
        public DumpHelper(Type objectType)
        {
            Guard.ArgumentIsNotNull(objectType, nameof(objectType));

            this.objectType = objectType;
            this.dumpMembersInfo = new SortedList<int, DumpSliceMemberInfo>();

            this.InitializeDumpMembersInfo();
            this.SetDumpMembersOffsets();
        }

        /// <summary>
        /// Gets the size of a manipulated object in memory.
        /// </summary>
        /// <value>Size of memory including extra memory areas.</value>
        public int SliceSize { get; private set; }

        /// <summary>
        ///     Updates object without updating an extra memory areas.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object for working with file.</param>
        /// <param name="obj">Object, which must be replaced with the old one.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="obj"/> is not of type, which was passed to the constructor.</exception>
        public void Update(Stream stream, object obj)
        {
            if (!this.objectType.Equals(obj.GetType()))
            {
                throw new ArgumentException($"Object must be of type {this.objectType.FullName}.", nameof(obj));
            }

            byte[] buffer = new byte[this.SliceSize - this.precedingAreaSize];
            stream.Seek(this.precedingAreaSize, SeekOrigin.Current);

            var offset = 0;
            foreach (var dumpMember in this.dumpMembersInfo)
            {
                if (dumpMember.Value.HolderName is null)
                {
                    continue;
                }

                var converter = this.getBytesConverters[dumpMember.Value.HolderType!];
                var value = this.objectType.GetProperty(dumpMember.Value.HolderName!, this.bindingFlags) !.GetValue(obj);

                var valueDump = converter(value!);
                for (int i = 0; i < valueDump.Length; i++)
                {
                    buffer[offset + i] = valueDump[i];
                }

                offset += dumpMember.Value.SizeInBytes;
            }

            stream.Write(buffer);
        }

        /// <summary>
        ///     Reads an object from the current stream's position.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object for working with file.</param>
        /// <returns>Object, that was read from the stream.</returns>
        public object? Read(Stream stream)
        {
            byte[] buffer = new byte[this.SliceSize];

            stream.Read(buffer);

            var result = Activator.CreateInstance(this.objectType);

            var offset = 0;
            foreach (var dumpMember in this.dumpMembersInfo)
            {
                if (dumpMember.Value.HolderName is null)
                {
                    offset += dumpMember.Value.SizeInBytes;
                    continue;
                }

                var converter = this.toValueConverters[dumpMember.Value.HolderType!];
                var value = converter(buffer, offset, dumpMember.Value.SizeInBytes);
                this.objectType.GetProperty(dumpMember.Value.HolderName, this.bindingFlags) !.SetValue(result, value);
                offset += dumpMember.Value.SizeInBytes;
            }

            return result;
        }

        /// <summary>
        ///     Gets an offset in bytes of memory area.
        /// </summary>
        /// <param name="areaName">Memory area name.</param>
        /// <returns>Offset in bytes of memory area.</returns>
        public int GetOffset(string areaName)
        {
            foreach (var memoryArea in this.dumpMembersInfo)
            {
                if (memoryArea.Value.Name.Equals(areaName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return memoryArea.Value.Offset;
                }
            }

            throw new ArgumentException($"Area with name '{areaName}' does not exist.", nameof(areaName));
        }

        /// <summary>
        ///     Gets an size in bytes of memory area.
        /// </summary>
        /// <param name="areaName">Memory area name.</param>
        /// <returns>Size in bytes of memory area.</returns>
        public int GetSize(string areaName)
        {
            foreach (var memoryArea in this.dumpMembersInfo)
            {
                if (memoryArea.Value.Name.Equals(areaName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return memoryArea.Value.SizeInBytes;
                }
            }

            throw new ArgumentException($"Area with name '{areaName}' does not exist.", nameof(areaName));
        }

        /// <summary>
        ///     Insert a dump of an object into a stream.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object for working with file.</param>
        /// <param name="obj">Object, which must be placed.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="obj"/> is not of type, which was passed to the constructor.</exception>
        public void Create(Stream stream, object obj)
        {
            if (!this.objectType.Equals(obj.GetType()))
            {
                throw new ArgumentException($"Object must be of type {this.objectType.FullName}.", nameof(obj));
            }

            byte[] buffer = new byte[this.SliceSize];
            var offset = 0;
            foreach (var dumpMember in this.dumpMembersInfo)
            {
                if (dumpMember.Value.HolderName is null)
                {
                    offset += dumpMember.Value.SizeInBytes;
                    continue;
                }

                var converter = this.getBytesConverters[dumpMember.Value.HolderType!];
                var value = this.objectType.GetProperty(dumpMember.Value.HolderName!, this.bindingFlags) !.GetValue(obj);

                var valueDump = converter(value!);
                for (int i = 0; i < valueDump.Length; i++)
                {
                    buffer[offset + i] = valueDump[i];
                }

                offset += dumpMember.Value.SizeInBytes;
            }

            stream.Write(buffer);
        }

        private void SetDumpMembersOffsets()
        {
            int memberOffset = 0;
            foreach (var dumpMemberInfo in this.dumpMembersInfo)
            {
                var dumpMember = dumpMemberInfo.Value;

                dumpMember.Offset = memberOffset;

                memberOffset += dumpMember.SizeInBytes;
                this.SliceSize += dumpMember.SizeInBytes;
            }
        }

        private void InitializeDumpMembersInfo()
        {
            var classAttribute = Attribute.GetCustomAttribute(this.objectType, typeof(DumpSliceAttribute)) as DumpSliceAttribute;

            if (classAttribute is null)
            {
                throw new ArgumentException($"Class {this.objectType.FullName} is not marked with ${typeof(DumpSliceAttribute).FullName}.");
            }

            if (classAttribute.PrecedingAreaSize != 0)
            {
                this.dumpMembersInfo.Add(1, new DumpSliceMemberInfo(classAttribute.PrecedingAreaName, classAttribute.PrecedingAreaSize, null, typeof(byte[])));
                this.precedingAreaSize = classAttribute.PrecedingAreaSize;
            }

            var members = this.objectType.GetMembers();
            foreach (var member in members)
            {
                var propertyAttribute = Attribute.GetCustomAttribute(member, typeof(DumpSliceMemberAttribute)) as DumpSliceMemberAttribute;
                if (propertyAttribute is not null)
                {
                    Type holderType = (member as PropertyInfo) !.PropertyType;

                    if (!this.getBytesConverters.ContainsKey(holderType))
                    {
                        throw new ArgumentException($"Type {holderType.FullName} is not supported!.");
                    }

                    this.dumpMembersInfo.Add(propertyAttribute.Order, new DumpSliceMemberInfo(propertyAttribute.Name, propertyAttribute.SizeInBytes, member.Name, holderType));
                }
            }
        }
    }
}
