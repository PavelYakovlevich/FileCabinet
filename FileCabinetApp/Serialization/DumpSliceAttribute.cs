using System;
using System.Collections.Generic;

namespace FileCabinetApp.Serialization
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DumpSliceAttribute : Attribute
    {
        public DumpSliceAttribute()
        {
            this.PrecedingAreaName = string.Empty;
            this.PrecedingAreaSize = 0;
        }

        public DumpSliceAttribute(string precedingSpaceName, int precedingSpaceSize)
            : this()
        {
            Guard.ArgumentGreaterThan(precedingSpaceSize, 0);

            this.PrecedingAreaName = precedingSpaceName;
            this.PrecedingAreaSize = precedingSpaceSize;
        }

        public string PrecedingAreaName { get; }

        public int PrecedingAreaSize { get; }
    }
}
