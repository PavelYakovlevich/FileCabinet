using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Serialization
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DumpSliceMemberAttribute : Attribute
    {
        public DumpSliceMemberAttribute(string name, int sizeInBytes, [CallerLineNumber] int order = 1)
        {
            this.Name = name;
            this.SizeInBytes = sizeInBytes;
            this.Order = order;
        }

        public int SizeInBytes { get; }

        public string Name { get; }

        public int Order { get; }
    }
}
