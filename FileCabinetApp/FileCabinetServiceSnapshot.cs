using System.IO;
using System.Xml;

using FileCabinetApp.Writers;

namespace FileCabinetApp
{
    /// <summary>
    ///     Class for the creating a <see cref="FileCabinetServiceSnapshot"/>.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="fileCabinetRecords"><see cref="System.Array"/> of file cabinet records.</param>
        public FileCabinetServiceSnapshot(FileCabinetRecord[] fileCabinetRecords)
        {
            Guard.ArgumentIsNotNull(fileCabinetRecords, nameof(fileCabinetRecords));

            this.records = fileCabinetRecords;
        }

        /// <summary>
        ///     Saves snapshot data into the csv file.
        /// </summary>
        /// <param name="writer"><see cref="StreamWriter"/> object to write to a stream.</param>
        public void SaveToCsv(StreamWriter writer)
        {
            var csvWriter = new FileCabinetRecordCsvWriter(writer);

            foreach (var record in this.records)
            {
                csvWriter.Write(record);
            }
        }

        /// <summary>
        ///     Saves snapshot data into the xml file.
        /// </summary>
        /// <param name="writer"><see cref="StreamWriter"/> object to write to a stream.</param>
        public void SaveToXml(StreamWriter writer)
        {
            using (var xmlTextWriter = new XmlTextWriter(writer))
            {
                xmlTextWriter.IndentChar = '\t';
                xmlTextWriter.Formatting = Formatting.Indented;

                xmlTextWriter.WriteStartDocument();
                xmlTextWriter.WriteStartElement("records");

                var xmlWriter = new FileCabinetRecordXmlWriter(xmlTextWriter);
                foreach (var record in this.records)
                {
                    xmlWriter.Write(record);
                }

                xmlTextWriter.WriteEndElement();
            }
        }
    }
}
