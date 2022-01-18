using System;

namespace FileCabinetApp.Serialization
{
    /// <summary>
    ///     Specifies extra memory areas, which can be placed before an serialized object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DumpSliceAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DumpSliceAttribute"/> class.
        /// </summary>
        public DumpSliceAttribute()
        {
            this.PrecedingAreaName = string.Empty;
            this.PrecedingAreaSize = 0;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DumpSliceAttribute"/> class.
        /// </summary>
        /// <param name="precedingSpaceName">Name of preceding extra memory area.</param>
        /// <param name="precedingSpaceSize">Size of preceding extra memory area.</param>
        public DumpSliceAttribute(string precedingSpaceName, int precedingSpaceSize)
            : this()
        {
            Guard.ArgumentGreaterThan(precedingSpaceSize, 0);

            this.PrecedingAreaName = precedingSpaceName;
            this.PrecedingAreaSize = precedingSpaceSize;
        }

        /// <summary>
        /// Gets a name of preceding extra memory area.
        /// </summary>
        /// <value>Name of preceding extra memory area.</value>
        public string PrecedingAreaName { get; }

        /// <summary>
        /// Gets a size of preceding extra memory area.
        /// </summary>
        /// <value>Size of preceding extra memory area.</value>
        public int PrecedingAreaSize { get; }
    }
}
