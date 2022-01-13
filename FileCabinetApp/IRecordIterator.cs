﻿namespace FileCabinetApp
{
    public interface IRecordIterator
    {
        public FileCabinetRecord GetNext();

        public bool HasMore();
    }
}
