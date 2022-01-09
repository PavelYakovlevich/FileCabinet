using System;
using System.Collections.Generic;

namespace FileCabinetApp.Printers
{
    /// <summary>
    ///     Default record printer.
    /// </summary>
    public class DefaultRecordPrinter : IRecordPrinter
    {
        private static string printFormat = "|{0,6}|{1,20}|{2,20}|{3}|{4}|{5,3}|{6}";

        /// <inheritdoc cref="IRecordPrinter.Print(IEnumerable{FileCabinetRecord})"/>
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            foreach (var record in records)
            {
                Console.WriteLine(printFormat, record.Id, record.FirstName, record.LastName, record.DateOfBirth.ToString("MM/dd/yyyy"), record.Gender, record.Stature, record.Weight);
            }
        }
    }
}
