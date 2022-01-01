using System;

namespace FileCabinetApp.Serialization
{
    public partial class DumpHelper
    {
        private class DumpSliceMemberInfo
        {
            private int offset;

            public DumpSliceMemberInfo(string name, int sizeInBytes, string? holderName, Type? holderType)
            {
                this.Name = name;
                this.SizeInBytes = sizeInBytes;
                this.HolderName = holderName;
                this.HolderType = holderType;
            }

            public string Name { get; }

            public int SizeInBytes { get; }

            public int Offset
            {
                get
                {
                    return this.offset;
                }

                set
                {
                    if (value < 0)
                    {
                        throw new ArgumentException($"{nameof(this.offset)} must be greater than 0", nameof(value));
                    }

                    this.offset = value;
                }
            }

            public string? HolderName { get; }

            public Type? HolderType { get; }
        }
    }
}
