using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    public class MemoryIterator : IRecordIterator
    {
        private readonly IList<FileCabinetRecord> records;

        private int currentElementIndex = 0;

        public MemoryIterator(IList<FileCabinetRecord> records)
        {
            Guard.ArgumentIsNotNull(records, nameof(records));

            this.records = records;
        }

        public FileCabinetRecord GetNext()
        {
            if (!this.HasMore())
            {
                throw new InvalidOperationException("There is no current item.");
            }

            return this.records[this.currentElementIndex++];
        }

        public bool HasMore()
        {
            return this.currentElementIndex < this.records.Count;
        }
    }
}
