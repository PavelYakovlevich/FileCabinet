using System;
using System.Collections.Generic;
using System.IO;

namespace FileCabinetApp.Readers
{
    /// <summary>
    ///     Class for a reading <see cref="FileCabinetRecord"/> objects from a csv file.
    /// </summary>
    public class FileCabinetRecordCsvReader : IFileCabinetRecordReader
    {
        private readonly StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.
        /// </summary>
        /// <param name="reader"><see cref="StreamReader"/> object, from which records will be read.</param>
        public FileCabinetRecordCsvReader(StreamReader reader)
        {
            Guard.ArgumentIsNotNull(reader, nameof(reader));

            this.reader = reader;
        }

        /// <inheritdoc cref="IFileCabinetRecordReader.ReadAll"/>
        public IList<FileCabinetRecord> ReadAll()
        {
            var result = new List<FileCabinetRecord>();

            var line = this.reader.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                var record = this.Parse(line);
                result.Add(record);

                line = this.reader.ReadLine();
            }

            return result;
        }

        private FileCabinetRecord Parse(string csvString)
        {
            var values = csvString.Split(',');

            return new FileCabinetRecord
            {
                Id = int.Parse(values[0]),
                FirstName = values[1],
                LastName = values[2],
                DateOfBirth = DateTime.Parse(values[3]),
                Gender = char.Parse(values[4]),
                Stature = short.Parse(values[5]),
                Weight = decimal.Parse(values[6]),
            };
        }
    }
}