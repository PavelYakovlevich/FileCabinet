using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FileCabinetApp.Readers
{
    public class FileCabinetRecordXmlReader : IFileCabinetRecordReader
    {
        private StreamReader reader;

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
