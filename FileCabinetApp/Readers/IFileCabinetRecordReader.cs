using System.Collections.Generic;

namespace FileCabinetApp.Readers
{
    /// <summary>
    ///     Interface for records readers.
    /// </summary>
    public interface IFileCabinetRecordReader
    {
        /// <summary>
        ///     Reads all records from a file.
        /// </summary>
        /// <returns><see cref="IList{FileCabinetRecord}"/> object with all records, extracted from a file.</returns>
        IList<FileCabinetRecord> ReadAll();
    }
}
