using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FileCabinetApp.Services
{
    /// <summary>
    ///     Class for the file cabinet's services.
    /// </summary>
    public class FileCabinetService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> existingRecords = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameSearchDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameSearchDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthSearchDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        private readonly IRecordValidator recordValidator;

        /// <summary>
            /// Initializes a new instance of the <see cref="FileCabinetService"/> class.
        /// </summary>
        /// <param name="recordValidator">Validator for the <see cref="FileCabinetRecordParameterObject"/>.</param>
        public FileCabinetService(IRecordValidator recordValidator)
        {
            this.recordValidator = recordValidator;
        }

        /// <inheritdoc cref="IFileCabinetService.CreateRecord(FileCabinetRecordParameterObject)"/>
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
            this.recordValidator.ValidateParameters(parameterObject);

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

        /// <inheritdoc cref="IFileCabinetService.GetStat"/>
        public int GetStat()
        {
            return this.existingRecords.Count;
        }

        /// <inheritdoc cref="IFileCabinetService.EditRecord(FileCabinetRecordParameterObject)"/>
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
            this.recordValidator.ValidateParameters(parameterObject);

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

        /// <inheritdoc cref="IFileCabinetService.RecordExists(int)"/>
        /// <exception cref="ArgumentException">Thrown when id is less than 1.</exception>
        public bool RecordExists(int id)
        {
            Guard.ArgumentGreaterThan(id, 0);

            return this.GetRecordById(id) is not null;
        }

        /// <inheritdoc cref="IFileCabinetService.GetRecords"/>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return new ReadOnlyCollection<FileCabinetRecord>(this.existingRecords);
        }

        /// <inheritdoc cref="IFileCabinetService.FindByFirstName"/>
        /// <exception cref="ArgumentNullException">Thrown when firstName is null.</exception>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            Guard.ArgumentIsNotNull(firstName, nameof(firstName));

            if (!this.firstNameSearchDictionary.ContainsKey(firstName))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(Array.Empty<FileCabinetRecord>());
            }

            return new ReadOnlyCollection<FileCabinetRecord>(this.firstNameSearchDictionary[firstName]);
        }

        /// <inheritdoc cref="IFileCabinetService.FindByLastName(string)"/>
        /// <exception cref="ArgumentNullException">Thrown when lastName is null.</exception>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            Guard.ArgumentIsNotNull(lastName, nameof(lastName));

            if (!this.lastNameSearchDictionary.ContainsKey(lastName))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(Array.Empty<FileCabinetRecord>());
            }

            return new ReadOnlyCollection<FileCabinetRecord>(this.lastNameSearchDictionary[lastName]);
        }

        /// <inheritdoc cref="IFileCabinetService.FindByDateOfBirth(DateTime)"/>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            if (!this.dateOfBirthSearchDictionary.ContainsKey(dateOfBirth))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(Array.Empty<FileCabinetRecord>());
            }

            return new ReadOnlyCollection<FileCabinetRecord>(this.dateOfBirthSearchDictionary[dateOfBirth]);
        }

        /// <inheritdoc cref="IFileCabinetService.MakeSnapshot"/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.existingRecords.ToArray());
        }

        private FileCabinetRecord? GetRecordById(int id)
        {
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
