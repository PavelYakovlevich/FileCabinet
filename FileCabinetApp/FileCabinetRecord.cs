using System;
using System.Xml.Serialization;
using FileCabinetApp.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    ///     Class for a user information.
    /// </summary>
    [DumpSlice("Reserved", sizeof(short))]
    [XmlType("record")]
    public class FileCabinetRecord
    {
        /// <summary>
        ///     Gets or sets id value of the user.
        /// </summary>
        /// <value>Id of the user.</value>
        [DumpSliceMember("Id", sizeof(int))]
        [XmlAttribute("id")]
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="Name"/> object. For xml serialization purpose only.
        /// </summary>
        /// <value><see cref="Name"/> object for holding first and last names.</value>
        [XmlElement("name")]
        public Name FullName { get; set; } = new Name();

        /// <summary>
        ///     Gets or sets first name value of the user.
        /// </summary>
        /// <value>First name of the user.</value>
        [DumpSliceMember("FirstName", sizeof(char) * 60)]
        [XmlIgnore]
        public string FirstName
        {
            get
            {
                return this.FullName.FirstName;
            }

            set
            {
                this.FullName.FirstName = value;
            }
        }

        /// <summary>
        ///     Gets or sets last name value of the user.
        /// </summary>
        /// <value>Last name of the user.</value>
        [DumpSliceMember("LastName", sizeof(char) * 60)]
        [XmlIgnore]
        public string LastName
        {
            get
            {
                return this.FullName.LastName;
            }

            set
            {
                this.FullName.LastName = value;
            }
        }

        /// <summary>
        ///     Gets or sets date of birth value of the user.
        /// </summary>
        /// <value>Date of birth of the user.</value>
        [DumpSliceMember("DateOfBirth", sizeof(int) * 3)]
        [XmlElement("dateOfBirth", DataType = "date")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        ///     Gets or sets stature value of the user.
        /// </summary>
        /// <value>Stature of the user.</value>
        [DumpSliceMember("Stature", sizeof(short))]
        [XmlElement("stature")]
        public short Stature { get; set; }

        /// <summary>
        ///     Gets or sets gender value of the user.
        /// </summary>
        /// <value>Gender of the user.</value>
        [DumpSliceMember("Gender", sizeof(char))]
        [XmlIgnore]
        public char Gender { get; set; }

        /// <summary>
        ///     Gets or sets weight value of the user.
        /// </summary>
        /// <value>Weight of the user.</value>
        [DumpSliceMember("Weight", sizeof(decimal))]
        [XmlElement("weight")]
        public decimal Weight { get; set; }

        /// <summary>
        ///     Gets or sets gender value of the user. For xml serialization purpose only.
        /// </summary>
        /// <value>Gender string of the user.</value>
        [XmlElement("gender")]
        public string GenderString
        {
            get
            {
                return this.Gender.ToString();
            }

            set
            {
                this.Gender = value[0];
            }
        }

        /// <summary>
        ///     Transform object into the string representation.
        /// </summary>
        /// <returns>String representation of the object<see cref="string"/>.</returns>
        public override string ToString()
        {
            return $"{this.Id}, {this.FirstName}, {this.LastName}, {this.DateOfBirth.ToString("yyyy-MMM-dd")}, {this.Gender}, {this.Stature}, {this.Weight}";
        }

        /// <summary>
        ///     Transform object into the csv string.
        /// </summary>
        /// <param name="dateFormat">Format of date of birth.</param>
        /// <returns>Csv string with all object's properties.</returns>
        public string ToCsvString(string dateFormat = "MM/dd/yyyy")
        {
            return $"{this.Id},{this.FirstName},{this.LastName},{this.DateOfBirth.ToString(dateFormat)},{this.Gender},{this.Stature},{this.Weight}";
        }

        /// <summary>
        ///     Class for holding first and last names of <see cref="FileCabinetRecord"/> object.
        /// </summary>
        public class Name
        {
            /// <summary>
            ///     Gets or sets first name.
            /// </summary>
            /// <value>First name.</value>
            [XmlAttribute("first")]
            public string FirstName { get; set; } = string.Empty;

            /// <summary>
            ///     Gets or sets last name.
            /// </summary>
            /// <value>Last name.</value>
            [XmlAttribute("last")]
            public string LastName { get; set; } = string.Empty;
        }
    }
}
