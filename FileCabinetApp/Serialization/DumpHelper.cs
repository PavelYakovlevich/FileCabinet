using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using FileCabinetApp.Utils;

namespace FileCabinetApp.Serialization
{
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
            { typeof(DateTime), (date) =>
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
            { typeof(DateTime), (bytes, offset, length) =>
                {
                    var year = BitConverter.ToInt32(bytes, offset);
                    var month = BitConverter.ToInt32(bytes, offset + sizeof(int));
                    var day = BitConverter.ToInt32(bytes, offset + (sizeof(int) * 2));
                    return new DateTime(year, month, day);
                }
            },
            { typeof(byte[]), (bytes, offset, length) => (byte[])bytes },
        };

        private SortedList<int, DumpSliceMemberInfo> dumpMembersInfo;

        public DumpHelper(Type objectType)
        {
            Guard.ArgumentIsNotNull(objectType, nameof(objectType));

            this.objectType = objectType;

            this.InitializeDumpMembersInfo();
            this.SetDumpMembersOffsets();
        }

        public int SliceSize { get; private set; }

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
                this.objectType.GetProperty(dumpMember.Value.HolderName) !.SetValue(result, value);
                offset += dumpMember.Value.SizeInBytes;
            }

            return result;
        }

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

        public void Write(Stream stream, object obj)
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
                var value = this.objectType.GetProperty(dumpMember.Value.HolderName!) !.GetValue(obj);

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

            this.dumpMembersInfo = new SortedList<int, DumpSliceMemberInfo>();
            if (classAttribute.PrecedingAreaSize != 0)
            {
                this.dumpMembersInfo.Add(1, new DumpSliceMemberInfo(classAttribute.PrecedingAreaName, classAttribute.PrecedingAreaSize, null, typeof(byte[])));
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
