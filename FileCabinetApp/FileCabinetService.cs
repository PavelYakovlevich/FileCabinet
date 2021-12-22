using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    /// <summary>
    ///     Class for the file cabinet's services.
    /// </summary>
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> existingRecords = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameSearchDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameSearchDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthSearchDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        /// <summary>
        ///     Creates a file cabinet record with the specified property's values.
        /// </summary>
        /// <param name="firstName">User's first name of type <see cref="string"/>.</param>
        /// <param name="lastName">User's last name of type <see cref="string"/>.</param>
        /// <param name="dateOfBirth">User's birthday of type <see cref="DateTime"/>.</param>
        /// <param name="gender">User's gender of type <see cref="char"/>.</param>
        /// <param name="weight">User's weight of type <see cref="decimal"/>.</param>
        /// <param name="stature">User's stature of type <see cref="short"/>.</param>
        /// <returns>Id of new file cabinet record.</returns>
        /// <exception cref="ArgumentNullException">Thrown when firstName of lastName is null.</exception>
        /// <exception cref="ArgumentException">
        ///     Throw when:
        ///         - firstName of lastName is empty or whitespace;
        ///         - firstName's or lastName's length is lower less than 2 or greater than 60;
        ///         - dateOfBirth is less than 1-Jan-1950 or greater than todays date;
        ///         - weight is lower than 0;
        ///         - stature is lower than 0.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when gender is not equal to M or F.</exception>
        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, char gender, decimal weight, short stature)
        {
            this.CheckInputParameters(firstName, lastName, dateOfBirth, gender, weight, stature);

            var newRecord = new FileCabinetRecord
            {
                Id = this.existingRecords.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Gender = gender,
                Weight = weight,
                Stature = stature,
            };

            this.existingRecords.Add(newRecord);

            this.AddSearchEntry(firstName, this.firstNameSearchDictionary, newRecord);
            this.AddSearchEntry(lastName, this.lastNameSearchDictionary, newRecord);
            this.AddSearchEntry(dateOfBirth, this.dateOfBirthSearchDictionary, newRecord);

            return newRecord.Id;
        }

        /// <summary>
        ///     Gets count of all existing file cabinet records.
        /// </summary>
        /// <returns>Count of all existing file cabinet records.</returns>
        public int GetStat()
        {
            return this.existingRecords.Count;
        }

        /// <summary>
        ///     Edits a file cabinet record with the specified property's values.
        /// </summary>
        /// <param name="id">Id of the existing file cabinet record<see cref="int"/>.</param>
        /// <param name="firstName">New user's first name of type <see cref="string"/>.</param>
        /// <param name="lastName">New User's last name of type <see cref="string"/>.</param>
        /// <param name="dateOfBirth">New User's birthday of type <see cref="DateTime"/>.</param>
        /// <param name="gender">New User's gender of type <see cref="char"/>.</param>
        /// <param name="weight">New User's weight of type <see cref="decimal"/>.</param>
        /// <param name="stature">New User's stature of type <see cref="short"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when firstName of lastName is null.</exception>
        /// <exception cref="ArgumentException">
        ///     Throw when:
        ///         - firstName of lastName is empty or whitespace;
        ///         - firstName's or lastName's length is lower less than 2 or greater than 60;
        ///         - dateOfBirth is less than 1-Jan-1950 or greater than todays date;
        ///         - weight is lower than 0;
        ///         - stature is lower than 0;
        ///         - file cabinet record with specified id not exists.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when gender is not equal to M or F.</exception>
        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, char gender, decimal weight, short stature)
        {
            this.CheckInputParameters(firstName, lastName, dateOfBirth, gender, weight, stature);

            var editableRecord = this.GetRecordById(id);

            Guard.ArgumentSatisfies(editableRecord, (editableRecord) => editableRecord is not null, $"User with {id} does not exist!");

            editableRecord!.FirstName = firstName;
            editableRecord!.LastName = lastName;
            editableRecord!.DateOfBirth = dateOfBirth;
            editableRecord!.Gender = gender;
            editableRecord!.Weight = weight;
            editableRecord!.Stature = stature;

            this.UpdateSearchEntry(firstName, this.firstNameSearchDictionary, editableRecord);
            this.UpdateSearchEntry(lastName, this.lastNameSearchDictionary, editableRecord);
            this.UpdateSearchEntry(dateOfBirth, this.dateOfBirthSearchDictionary, editableRecord);
        }

        /// <summary>
        ///     Checks if file cabinet record with specified <paramref name="id"/> exists.
        /// </summary>
        /// <param name="id">Id of the file cabinet record.</param>
        /// <returns>True if file cabinet record with specified id exists, overwise false.</returns>
        public bool RecordExists(int id)
        {
            return this.GetRecordById(id) is not null;
        }

        /// <summary>
        ///     Gets array of all existing <see cref="FileCabinetRecord"/>.
        /// </summary>
        /// <returns>Array of all existing <see cref="FileCabinetRecord"/>.</returns>
        public FileCabinetRecord[] GetRecords()
        {
            return this.existingRecords.ToArray();
        }

        /// <summary>
        ///     Perfoms search of all existing <see cref="FileCabinetRecord"/> that have first name's value equal to the specified <paramref name="firstName"/>.
        /// </summary>
        /// <param name="firstName">First name search value.</param>
        /// <returns>All <see cref="FileCabinetRecord"/> records, which have the same first name value as <paramref name="firstName"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when firstName is null.</exception>
        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            Guard.ArgumentIsNotNull(firstName, nameof(firstName));

            if (!this.firstNameSearchDictionary.ContainsKey(firstName))
            {
                return Array.Empty<FileCabinetRecord>();
            }

            return this.firstNameSearchDictionary[firstName].ToArray();
        }

        /// <summary>
        ///     Perfoms search of all existing <see cref="FileCabinetRecord"/> that have last name's value equal to the specified <paramref name="lastName"/>.
        /// </summary>
        /// <param name="lastName">Last name search value.</param>
        /// <returns>All <see cref="FileCabinetRecord"/> records, which have the same last name value as <paramref name="lastName"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when lastName is null.</exception>
        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            Guard.ArgumentIsNotNull(lastName, nameof(lastName));

            if (!this.firstNameSearchDictionary.ContainsKey(lastName))
            {
                return Array.Empty<FileCabinetRecord>();
            }

            return this.firstNameSearchDictionary[lastName].ToArray();
        }

        /// <summary>
        ///     Perfoms search of all existing <see cref="FileCabinetRecord"/> that have date of birth's value equal to the specified <paramref name="dateOfBirth"/>.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth search value.</param>
        /// <returns>All <see cref="FileCabinetRecord"/> records, which have the same birthday value as <paramref name="dateOfBirth"/>.</returns>
        public FileCabinetRecord[] FindByDateOfBirth(DateTime dateOfBirth)
        {
            if (!this.dateOfBirthSearchDictionary.ContainsKey(dateOfBirth))
            {
                return Array.Empty<FileCabinetRecord>();
            }

            return this.dateOfBirthSearchDictionary[dateOfBirth].ToArray();
        }

        private FileCabinetRecord? GetRecordById(int id)
        {
            Guard.ArgumentGreaterThan(id, 0);

            foreach (var record in this.existingRecords)
            {
                if (record.Id == id)
                {
                    return record;
                }
            }

            return null;
        }

        private void AddSearchEntry<T>(T key, Dictionary<T, List<FileCabinetRecord>> searchDictionary, FileCabinetRecord record)
            where T : notnull
        {
            if (!searchDictionary.ContainsKey(key))
            {
                searchDictionary.Add(key, new List<FileCabinetRecord>());
            }

            var referenceList = searchDictionary[key];
            referenceList.Add(record);
        }

        private void UpdateSearchEntry<T>(T key, Dictionary<T, List<FileCabinetRecord>> searchDictionary, FileCabinetRecord record)
            where T : notnull
        {
            foreach (var searchEntry in searchDictionary)
            {
                var referenceList = searchEntry.Value;

                if (referenceList.Contains(record))
                {
                    referenceList.Remove(record);
                }
            }

            this.AddSearchEntry(key, searchDictionary, record);
        }

        private void CheckInputParameters(string firstName, string lastName, DateTime dateOfBirth, char gender, decimal weight, short stature)
        {
            Guard.ArgumentIsNotNull(firstName, nameof(firstName));
            Guard.ArgumentIsNotEmptyOrWhiteSpace(firstName, nameof(firstName));
            Guard.ArgumentSatisfies(
                firstName,
                (firstName) => firstName.Length >= 2 && firstName.Length <= 60,
                $"{nameof(firstName)} lenght must be greater than 1 and less than 61.");

            Guard.ArgumentIsNotNull(lastName, nameof(lastName));
            Guard.ArgumentIsNotEmptyOrWhiteSpace(lastName, nameof(lastName));
            Guard.ArgumentSatisfies(
                lastName,
                (lastName) => lastName.Length >= 2 && lastName.Length <= 60,
                $"{nameof(lastName)} lenght must be greater than 1 and less than 61.");

            Guard.ArgumentSatisfies(
                dateOfBirth,
                (dateOfBirth) => dateOfBirth.CompareTo(new DateTime(1950, 1, 1)) >= 0 && dateOfBirth.CompareTo(DateTime.Now) <= 0,
                $"{nameof(dateOfBirth)} must be greater than 01-Jan-1950 and less or equal to current date.");

            Guard.ArgumentIsInRange(gender, new[] { 'M', 'F' });

            Guard.ArgumentGreaterThan(weight, 0);

            Guard.ArgumentGreaterThan(stature, 0);
        }
    }
}
