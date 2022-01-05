using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using FileCabinetApp.Readers;
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
        public FileCabinetServiceSnapshot()
        {
            this.records = Array.Empty<FileCabinetRecord>();
        }

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
        /// Gets all file cabinet records, which are saved in the snapshot.
        /// </summary>
        /// <value>All file cabinet records, which are saved in the snapshot.</value>
        public ReadOnlyCollection<FileCabinetRecord> Records
        {
            get
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.records);
            }
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
        ///     Loads records from a csv file.
        /// </summary>
        /// <param name="reader">><see cref="StreamReader"/> object to read from a stream.</param>
        public void LoadFromCsv(StreamReader reader)
        {
            var csvReader = new FileCabinetRecordCsvReader(reader);

            var loadedRecords = csvReader.ReadAll();

            Array.Resize(ref this.records, loadedRecords.Count);
            loadedRecords.CopyTo(this.records, 0);
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

        /// <summary>
        ///     Loads records from a xml file.
        /// </summary>
        /// <param name="reader">><see cref="StreamReader"/> object to read from a stream.</param>
        public void LoadFromXml(StreamReader reader)
        {
            var xmlReader = new FileCabinetRecordXmlReader(reader);

            var loadedRecords = xmlReader.ReadAll();

            Array.Resize(ref this.records, loadedRecords.Count);
            loadedRecords.CopyTo(this.records, 0);
        }
    }
}
