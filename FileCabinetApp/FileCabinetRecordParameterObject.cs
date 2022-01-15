using System;

#pragma warning disable CS8618

namespace FileCabinetApp
{
    /// <summary>
    ///     Class for the creation and editing input parameters.
    /// </summary>
    public class FileCabinetRecordParameterObject
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FileCabinetRecordParameterObject"/> class.
        /// </summary>
        public FileCabinetRecordParameterObject()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileCabinetRecordParameterObject"/> class.
        /// </summary>
        /// <param name="firstName">User's first name property value.</param>
        /// <param name="lastName">User's last name property value.</param>
        /// <param name="dateOfBirth">User's date of birth property value.</param>
        /// <param name="gender">User's gender property value.</param>
        /// <param name="weight">User's weight property value.</param>
        /// <param name="stature">User's stature property value.</param>
        public FileCabinetRecordParameterObject(string firstName, string lastName, DateTime dateOfBirth, short stature, char gender, decimal weight)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Stature = stature;
            this.Gender = gender;
            this.Weight = weight;
        }

        /// <inheritdoc cref="FileCabinetRecordParameterObject"/>
        /// <param name="id">Id of the file cabinet record.</param>
        public FileCabinetRecordParameterObject(int id, string firstName, string lastName, DateTime dateOfBirth, short stature, char gender, decimal weight)
            : this(firstName, lastName, dateOfBirth, stature, gender, weight)
        {
            this.Id = id;
        }

        /// <summary>
        ///     Gets or sets id value of the user.
        /// </summary>
        /// <value>Id of the user.</value>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets first name value of the user.
        /// </summary>
        /// <value>First name of the user.</value>
        public string FirstName { get; set; }

        /// <summary>
        ///     Gets or sets last name value of the user.
        /// </summary>
        /// <value>Last name of the user.</value>
        public string LastName { get; set; }

        /// <summary>
        ///     Gets or sets date of birth value of the user.
        /// </summary>
        /// <value>Date of birth of the user.</value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        ///     Gets or sets  stature value of the user.
        /// </summary>
        /// <value>Stature of the user.</value>
        public short Stature { get; set; }

        /// <summary>
        ///     Gets or sets  gender value of the user.
        /// </summary>
        /// <value>Gender of the user.</value>
        public char Gender { get; set; }

        /// <summary>
        ///     Gets or sets  weight value of the user.
        /// </summary>
        /// <value>Weight of the user.</value>
        public decimal Weight { get; set; }
    }
}
