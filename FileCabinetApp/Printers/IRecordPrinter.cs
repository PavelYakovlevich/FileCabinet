using System.Collections.Generic;

namespace FileCabinetApp.Printers
{
    /// <summary>
    ///     Interface for printers of <see cref="FileCabinetApp"/> objects.
    /// </summary>
    public interface IRecordPrinter
    {
        /// <summary>
        ///     Prints all <paramref name="records"/>.
        /// </summary>
        /// <param name="records">Records to print.</param>
        void Print(IEnumerable<FileCabinetRecord> records);

        /// <summary>
        ///     Prints <see cref="FileCabinetRecord"/> object's data.
        /// </summary>
        /// <param name="record"><see cref="FileCabinetRecord"/> object to print.</param>
        void Print(FileCabinetRecord record);
    }
}
