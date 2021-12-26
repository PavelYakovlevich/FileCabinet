using System;

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
        ///     Gets id value of the user.
        /// </summary>
        /// <value>Id of the user.</value>
        public int Id { get; private set; }

        /// <summary>
        ///     Gets first name value of the user.
        /// </summary>
        /// <value>First name of the user.</value>
        public string FirstName { get; private set; }

        /// <summary>
        ///     Gets last name value of the user.
        /// </summary>
        /// <value>Last name of the user.</value>
        public string LastName { get; private set; }

        /// <summary>
        ///     Gets date of birth value of the user.
        /// </summary>
        /// <value>Date of birth of the user.</value>
        public DateTime DateOfBirth { get; private set; }

        /// <summary>
        ///     Gets stature value of the user.
        /// </summary>
        /// <value>Stature of the user.</value>
        public short Stature { get; private set; }

        /// <summary>
        ///     Gets gender value of the user.
        /// </summary>
        /// <value>Gender of the user.</value>
        public char Gender { get; private set; }

        /// <summary>
        ///     Gets weight value of the user.
        /// </summary>
        /// <value>Weight of the user.</value>
        public decimal Weight { get; private set; }
    }
}
