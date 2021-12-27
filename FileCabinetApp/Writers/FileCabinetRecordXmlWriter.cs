using System.Xml;

namespace FileCabinetApp.Writers
{
    /// <summary>
    ///     Class for the file cabinet record writting in xml format.
    /// </summary>
    public class FileCabinetRecordXmlWriter : IFileCabinetRecordWriter
    {
        private XmlWriter xmlWriter;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer"><see cref="xmlWriter"/> object.</param>
        public FileCabinetRecordXmlWriter(XmlWriter writer)
        {
            this.xmlWriter = writer;
        }

        /// <inheritdoc cref="IFileCabinetRecordWriter.Write(FileCabinetRecord)"/>
        public void Write(FileCabinetRecord record)
        {
            this.xmlWriter.WriteStartElement("record");

            this.xmlWriter.WriteAttributeString("id", record.Id.ToString());

            this.xmlWriter.WriteStartElement("name");
            this.xmlWriter.WriteAttributeString("first", record.FirstName);
            this.xmlWriter.WriteAttributeString("last", record.LastName);
            this.xmlWriter.WriteEndElement();

            this.xmlWriter.WriteElementString("dateOfBirth", record.DateOfBirth.ToString("MM/dd/yyyy"));

            this.xmlWriter.WriteElementString("stature", record.Stature.ToString());
            this.xmlWriter.WriteElementString("weight", record.Weight.ToString());
            this.xmlWriter.WriteElementString("gender", record.Gender.ToString());

            this.xmlWriter.WriteEndElement();
        }
    }
}
