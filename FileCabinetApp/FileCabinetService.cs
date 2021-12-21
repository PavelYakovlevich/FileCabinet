using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> existingRecords = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameSearchDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameSearchDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthSearchDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

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

        public int GetStat()
        {
            return this.existingRecords.Count;
        }

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

        public bool RecordExists(int id)
        {
            return this.GetRecordById(id) is not null;
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.existingRecords.ToArray();
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            Guard.ArgumentIsNotNull(firstName, nameof(firstName));

            if (!this.firstNameSearchDictionary.ContainsKey(firstName))
            {
                return Array.Empty<FileCabinetRecord>();
            }

            return this.firstNameSearchDictionary[firstName].ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            Guard.ArgumentIsNotNull(lastName, nameof(lastName));

            if (!this.firstNameSearchDictionary.ContainsKey(lastName))
            {
                return Array.Empty<FileCabinetRecord>();
            }

            return this.firstNameSearchDictionary[lastName].ToArray();
        }

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
