using System;
using System.Collections.Generic;

namespace FileCabinetApp.Services
{
    /// <summary>
    ///     Class for the file cabinet's services.
    /// </summary>
    public abstract class FileCabinetService
    {
        private readonly List<FileCabinetRecord> existingRecords = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameSearchDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameSearchDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthSearchDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        /// <summary>
        ///     Creates a file cabinet record with the specified property's values.
        /// </summary>
        /// <param name="parameterObject.">Parameter object, which holds all necessary data for the creation of <see cref="FileCabinetRecord"/> object.</param>
        /// <returns>Id of new file cabinet record.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameterObject"/>'s firstName or lastName is null.</exception>
        /// <exception cref="ArgumentException">
        ///     Throw when <paramref name="parameterObject"/> inner properties are not valid:
        ///         - firstName of lastName is empty or whitespace;
        ///         - firstName's or lastName's length is lower less than 2 or greater than 60;
        ///         - dateOfBirth is less than 1-Jan-1950 or greater than todays date;
        ///         - weight is lower than 0;
        ///         - stature is lower than 0;
        ///         - file cabinet record with specified id not exists.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="parameterObject"/>'s gender is not equal to M or F.</exception>
        public int CreateRecord(FileCabinetRecordParameterObject parameterObject)
        {
            this.CreateValidator().ValidateParameters(parameterObject);

            var newRecord = new FileCabinetRecord
            {
                Id = this.existingRecords.Count + 1,
                FirstName = parameterObject.FirstName,
                LastName = parameterObject.LastName,
                DateOfBirth = parameterObject.DateOfBirth,
                Gender = parameterObject.Gender,
                Weight = parameterObject.Weight,
                Stature = parameterObject.Stature,
            };

            this.existingRecords.Add(newRecord);

            this.AddSearchEntry(parameterObject.FirstName, this.firstNameSearchDictionary, newRecord);
            this.AddSearchEntry(parameterObject.LastName, this.lastNameSearchDictionary, newRecord);
            this.AddSearchEntry(parameterObject.DateOfBirth, this.dateOfBirthSearchDictionary, newRecord);

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
        /// <param name="parameterObject.">Parameter object, which holds all necessary data for the editting of <see cref="FileCabinetRecord"/> object.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameterObject"/>'s firstName or lastName is null.</exception>
        /// <exception cref="ArgumentException">
        ///     Throw when <paramref name="parameterObject"/> inner properties are not valid:
        ///         - firstName of lastName is empty or whitespace;
        ///         - firstName's or lastName's length is lower less than 2 or greater than 60;
        ///         - dateOfBirth is less than 1-Jan-1950 or greater than todays date;
        ///         - weight is lower than 0;
        ///         - stature is lower than 0;
        ///         - file cabinet record with specified id not exists.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="parameterObject"/>'s gender is not equal to M or F.</exception>
        public void EditRecord(FileCabinetRecordParameterObject parameterObject)
        {
            this.CreateValidator().ValidateParameters(parameterObject);

            var editableRecord = this.GetRecordById(parameterObject.Id);

            Guard.ArgumentSatisfies(editableRecord, (editableRecord) => editableRecord is not null, $"User with {parameterObject.Id} does not exist!");

            editableRecord!.FirstName = parameterObject.FirstName;
            editableRecord!.LastName = parameterObject.LastName;
            editableRecord!.DateOfBirth = parameterObject.DateOfBirth;
            editableRecord!.Gender = parameterObject.Gender;
            editableRecord!.Weight = parameterObject.Weight;
            editableRecord!.Stature = parameterObject.Stature;

            this.UpdateSearchEntry(parameterObject.FirstName, this.firstNameSearchDictionary, editableRecord);
            this.UpdateSearchEntry(parameterObject.LastName, this.lastNameSearchDictionary, editableRecord);
            this.UpdateSearchEntry(parameterObject.DateOfBirth, this.dateOfBirthSearchDictionary, editableRecord);
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

        protected abstract IRecordValidator CreateValidator();

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
    }
}
