using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FileCabinetApp.Utils;

namespace FileCabinetApp.Services
{
    /// <summary>
    ///     Class for the file cabinet's services.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> existingRecords = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameSearchDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameSearchDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthSearchDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        private readonly IRecordValidator recordValidator;
        private readonly Memoizer<SearchInfo<FileCabinetRecord>, IEnumerable<FileCabinetRecord>> memoizer;

        /// <summary>
            /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        /// <param name="recordValidator">Validator for the <see cref="FileCabinetRecordParameterObject"/>.</param>
        public FileCabinetMemoryService(IRecordValidator recordValidator)
        {
            this.recordValidator = recordValidator;
            this.memoizer = new Memoizer<SearchInfo<FileCabinetRecord>, IEnumerable<FileCabinetRecord>>(new SearchInfoComparer<FileCabinetRecord>());
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

            var recordId = 1;
            if (this.existingRecords.Count != 0)
            {
                recordId = this.existingRecords[this.existingRecords.Count - 1].Id + 1;
            }

            var newRecord = new FileCabinetRecord
            {
                Id = recordId,
                FirstName = parameterObject.FirstName,
                LastName = parameterObject.LastName,
                DateOfBirth = parameterObject.DateOfBirth,
                Gender = parameterObject.Gender,
                Weight = parameterObject.Weight,
                Stature = parameterObject.Stature,
            };

            this.existingRecords.Add(newRecord);

            this.AddEntryToSearchDictionaries(newRecord);

            this.memoizer.ClearCache();

            return newRecord.Id;
        }

        /// <inheritdoc cref="IFileCabinetService.GetStat"/>
        public (int total, int deleted) GetStat()
        {
            return (this.existingRecords.Count, 0);
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

            this.UpdateSearchDictionaries(editableRecord);

            this.memoizer.ClearCache();
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

        /// <inheritdoc cref="IFileCabinetService.Find(SearchInfo{FileCabinetRecord})"/>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="searchInfo"/> is null.</exception>
        public IEnumerable<FileCabinetRecord> Find(SearchInfo<FileCabinetRecord> searchInfo)
        {
            Guard.ArgumentIsNotNull(searchInfo, nameof(searchInfo));

            var memoizedFunc = this.memoizer.Memoize(this.FindBySearchInfo);

            return memoizedFunc(searchInfo);
        }

        /// <inheritdoc cref="IFileCabinetService.FindByFirstName"/>
        /// <exception cref="ArgumentNullException">Thrown when firstName is null.</exception>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            Guard.ArgumentIsNotNull(firstName, nameof(firstName));

            List<FileCabinetRecord>? records;
            if (!this.firstNameSearchDictionary.TryGetValue(firstName, out records))
            {
                records = new List<FileCabinetRecord>();
            }

            foreach (var record in records)
            {
                yield return record;
            }
        }

        /// <inheritdoc cref="IFileCabinetService.FindByLastName(string)"/>
        /// <exception cref="ArgumentNullException">Thrown when lastName is null.</exception>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            Guard.ArgumentIsNotNull(lastName, nameof(lastName));

            List<FileCabinetRecord>? records;
            if (!this.lastNameSearchDictionary.TryGetValue(lastName, out records))
            {
                records = new List<FileCabinetRecord>();
            }

            foreach (var record in records)
            {
                yield return record;
            }
        }

        /// <inheritdoc cref="IFileCabinetService.FindByDateOfBirth(DateTime)"/>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            List<FileCabinetRecord>? records;
            if (!this.dateOfBirthSearchDictionary.TryGetValue(dateOfBirth, out records))
            {
                records = new List<FileCabinetRecord>();
            }

            foreach (var record in records)
            {
                yield return record;
            }
        }


        /// <inheritdoc cref="IFileCabinetService.MakeSnapshot"/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.existingRecords.ToArray());
        }

        /// <inheritdoc cref="IFileCabinetService.Restore(FileCabinetServiceSnapshot, Action{FileCabinetRecord, string})"/>
        public int Restore(FileCabinetServiceSnapshot snapshot, Action<FileCabinetRecord, string> onInvalidRecordImported)
        {
            Guard.ArgumentIsNotNull(snapshot, nameof(snapshot));

            var records = snapshot.Records;
            var importedRecords = 0;
            foreach (var record in records)
            {
                try
                {
                    this.recordValidator.Validate(record);

                    var editableRecord = this.GetRecordById(record.Id);
                    if (editableRecord is null)
                    {
                        this.existingRecords.Add(record);

                        this.AddEntryToSearchDictionaries(record);
                    }
                    else
                    {
                        editableRecord!.FirstName = record.FirstName;
                        editableRecord!.LastName = record.LastName;
                        editableRecord!.DateOfBirth = record.DateOfBirth;
                        editableRecord!.Gender = record.Gender;
                        editableRecord!.Weight = record.Weight;
                        editableRecord!.Stature = record.Stature;

                        this.UpdateSearchDictionaries(record);
                    }

                    importedRecords++;
                }
                catch (ArgumentException exception)
                {
                    onInvalidRecordImported?.Invoke(record, exception.Message);
                }
            }

            return importedRecords;
        }

        /// <inheritdoc cref="IFileCabinetService.RemoveRecord(int)"/>
        public void RemoveRecord(int recordId)
        {
            Guard.ArgumentGreaterThan(recordId, 0, $"{nameof(recordId)} must be greater than 0.");

            var targetRecord = this.existingRecords.Find((record) => record.Id == recordId);
            if (targetRecord is null)
            {
                throw new ArgumentException($"Record #{recordId} does not exist.", nameof(recordId));
            }

            this.existingRecords.Remove(targetRecord);
            this.DeleteEntryFromSearchDictionaries(targetRecord);
            this.memoizer.ClearCache();
        }

        /// <summary>
        ///     Does nothing.
        /// </summary>
        public void Purge()
        {
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

        private void AddEntryToSearchDictionaries(FileCabinetRecord record)
        {
            this.AddSearchEntry(record.FirstName, this.firstNameSearchDictionary, record);
            this.AddSearchEntry(record.LastName, this.lastNameSearchDictionary, record);
            this.AddSearchEntry(record.DateOfBirth, this.dateOfBirthSearchDictionary, record);
        }

        private void UpdateSearchDictionaries(FileCabinetRecord record)
        {
            this.UpdateSearchEntry(record.FirstName, this.firstNameSearchDictionary, record);
            this.UpdateSearchEntry(record.LastName, this.lastNameSearchDictionary, record);
            this.UpdateSearchEntry(record.DateOfBirth, this.dateOfBirthSearchDictionary, record);
        }

        private void DeleteEntryFromSearchDictionaries(FileCabinetRecord record)
        {
            this.DeleteSearchEntry(record.FirstName, this.firstNameSearchDictionary, record);
            this.DeleteSearchEntry(record.LastName, this.lastNameSearchDictionary, record);
            this.DeleteSearchEntry(record.DateOfBirth, this.dateOfBirthSearchDictionary, record);
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
            this.DeleteSearchEntry(key, searchDictionary, record);
            this.AddSearchEntry(key, searchDictionary, record);
        }

        private void DeleteSearchEntry<T>(T key, Dictionary<T, List<FileCabinetRecord>> searchDictionary, FileCabinetRecord record)
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
        }

        private IEnumerable<FileCabinetRecord> FindBySearchInfo(SearchInfo<FileCabinetRecord> searchInfo)
        {
            if (searchInfo.SearchCriterias.ContainsKey("firstname"))
            {
                return this.FindByFirstName(searchInfo.SearchCriterias["firstname"][0]);
            }

            if (searchInfo.SearchCriterias.ContainsKey("lastname"))
            {
                return this.FindByLastName(searchInfo.SearchCriterias["lastname"][0]);
            }

            if (searchInfo.SearchCriterias.ContainsKey("dateofbirth"))
            {
                var dateOfBirth = DateTime.Parse(searchInfo.SearchCriterias["dateofbirth"][0]);
                return this.FindByDateOfBirth(dateOfBirth);
            }

            return this.existingRecords.FindAll(searchInfo.SearchPredicate);
        }
    }
}
