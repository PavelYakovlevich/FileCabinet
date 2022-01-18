using System.IO;

namespace FileCabinetApp.Writers
{
    /// <summary>
    ///     Class for savind a <see cref="FileCabinetRecord"/> object in csv format to a <see cref="TextWriter"/> stream.
    /// </summary>
    internal class FileCabinetRecordCsvWriter : IFileCabinetRecordWriter
    {
        private readonly TextWriter writer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer"><see cref="TextWriter"/> object.</param>
        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            Guard.ArgumentIsNotNull(writer, nameof(writer));

            this.writer = writer;
        }

        /// <inheritdoc cref="IFileCabinetRecordWriter.Write(FileCabinetRecord)"/>
        public void Write(FileCabinetRecord record)
        {
            Guard.ArgumentIsNotNull(record, nameof(record));

            var csvString = $"{record.Id},{record.FirstName},{record.LastName},{record.DateOfBirth.ToString("yyyy-MMM-dd")},{record.Gender},{record.Stature},{record.Weight}";

            this.writer.WriteLine(csvString);
        }
    }
}