using System;
using System.Runtime.CompilerServices;

namespace FileCabinetApp.Serialization
{
    /// <summary>
    ///     Specifies an information about named field of serializable object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DumpSliceMemberAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DumpSliceMemberAttribute"/> class.
        /// </summary>
        /// <param name="name">Name of field.</param>
        /// <param name="sizeInBytes">Size of field.</param>
        /// <param name="order">Order of member in memory.</param>
        public DumpSliceMemberAttribute(string name, int sizeInBytes, [CallerLineNumber] int order = 1)
        {
            this.Name = name;
            this.SizeInBytes = sizeInBytes;
            this.Order = order;
        }

        /// <summary>
        /// Gets a size of field in bytes.
        /// </summary>
        /// <value>Size of field in bytes</value>
        public int SizeInBytes { get; }

        /// <summary>
        /// Gets a name of field.
        /// </summary>
        /// <value>Name of field.</value>
        public string Name { get; }

        /// <summary>
        /// Gets an order of field on memory.
        /// </summary>
        /// <value>Order of field on memory.</value>
        public int Order { get; }
    }
}
