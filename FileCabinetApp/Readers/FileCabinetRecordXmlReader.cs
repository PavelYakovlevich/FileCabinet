using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FileCabinetApp.Readers
{
    /// <summary>
    ///     Class for a reading <see cref="FileCabinetRecord"/> objects from a xml file.
    /// </summary>
    public class FileCabinetRecordXmlReader : IFileCabinetRecordReader
    {
        private readonly StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="reader"><see cref="StreamReader"/> object, from which records will be read.</param>
        public FileCabinetRecordXmlReader(StreamReader reader)
        {
            Guard.ArgumentIsNotNull(reader, nameof(reader));

            this.reader = reader;
        }

        /// <inheritdoc cref="IFileCabinetRecordReader.ReadAll"/>
        public IList<FileCabinetRecord> ReadAll()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FileCabinetRecord[]), new XmlRootAttribute("records"));

            var records = serializer.Deserialize(this.reader);

            if (records is null)
            {
                return new List<FileCabinetRecord>();
            }

            return (IList<FileCabinetRecord>)records;
        }
    }
}