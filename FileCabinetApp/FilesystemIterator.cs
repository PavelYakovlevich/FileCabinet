using System;
using System.Collections.Generic;
using System.IO;
using FileCabinetApp.Serialization;

namespace FileCabinetApp
{
    public class FilesystemIterator : IRecordIterator
    {
        private readonly Stream stream;
        private readonly DumpHelper dumpHelper;

        private long currentRecodAddress = 0;

        private IList<long>? offsets;
        private int currentOffsetIndex = 0;

        public FilesystemIterator(Stream stream)
        {
            Guard.ArgumentIsNotNull(stream, nameof(stream));

            this.stream = stream;
            this.dumpHelper = new DumpHelper(typeof(FileCabinetRecord));
        }

        public FilesystemIterator(Stream stream, IList<long> offsets)
            : this(stream)
        {
            Guard.ArgumentIsNotNull(offsets, nameof(offsets));

            this.offsets = offsets;
        }

        public FileCabinetRecord GetNext()
        {
            if (!this.HasMore())
            {
                throw new InvalidOperationException("There is no more elements.");
            }

            if (this.offsets is not null)
            {
                this.currentRecodAddress = this.offsets[this.currentOffsetIndex];
                this.currentOffsetIndex++;
            }
            else
            {
                this.currentRecodAddress += this.dumpHelper.SliceSize;
            }

            this.stream.Seek(this.currentRecodAddress, SeekOrigin.Begin);

            var record = (FileCabinetRecord)this.dumpHelper.Read(this.stream);

            return record;
        }

        public bool HasMore()
        {
            if (this.offsets is not null)
            {
                return this.currentOffsetIndex < this.offsets.Count;
            }

            return this.currentRecodAddress < this.stream.Length;
        }
    }
}
