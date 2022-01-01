using FileCabinetApp.Serialization;
using System;

namespace FileCabinetApp
{
    /// <summary>
    ///     Class for a user information.
    /// </summary>
    [DumpSlice("Reserved", sizeof(short))]
    public class FileCabinetRecord
    {
        /// <summary>
        ///     Gets or sets id value of the user.
        /// </summary>
        /// <value>Id of the user.</value>
        [DumpSliceMember("Id", sizeof(int))]
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets first name value of the user.
        /// </summary>
        /// <value>First name of the user.</value>
        [DumpSliceMember("FirstName", sizeof(char) * 60)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets last name value of the user.
        /// </summary>
        /// <value>Last name of the user.</value>
        [DumpSliceMember("LastName", sizeof(char) * 60)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets date of birth value of the user.
        /// </summary>
        /// <value>Date of birth of the user.</value>
        [DumpSliceMember("DateOfBirth", sizeof(int) * 3)]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        ///     Gets or sets stature value of the user.
        /// </summary>
        /// <value>Stature of the user.</value>
        [DumpSliceMember("Stature", sizeof(short))]
        public short Stature { get; set; }

        /// <summary>
        ///     Gets or sets gender value of the user.
        /// </summary>
        /// <value>Gender of the user.</value>
        [DumpSliceMember("Gender", sizeof(char))]
        public char Gender { get; set; }

        /// <summary>
        ///     Gets or sets weight value of the user.
        /// </summary>
        /// <value>Weight of the user.</value>
        [DumpSliceMember("Weight", sizeof(decimal))]
        public decimal Weight { get; set; }

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
    }
}
