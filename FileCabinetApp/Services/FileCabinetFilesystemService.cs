using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using FileCabinetApp.Serialization;

namespace FileCabinetApp.Services
{
    /// <summary>
    ///     Service class for the work with a file system.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private readonly IRecordValidator recordValidator;
        private readonly Stream fileStream;
        private readonly DumpHelper dumpHelper;

        private readonly Dictionary<string, List<long>> firstNameSearchDictionary;
        private readonly Dictionary<string, List<long>> lastNameSearchDictionary;
        private readonly Dictionary<DateTime, List<long>> dateOfBirthSearchDictionary;

        private int lastRecordId;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="recordValidator">Validation function, which validates input parameter object.</param>
        /// <param name="stream"><see cref="Stream"/> object for working with a file system.</param>
        public FileCabinetFilesystemService(IRecordValidator recordValidator, Stream stream)
        {
            Guard.ArgumentIsNotNull(recordValidator, nameof(recordValidator));
            Guard.ArgumentIsNotNull(stream, nameof(stream));

            this.recordValidator = recordValidator;
            this.fileStream = stream;
            this.dumpHelper = new DumpHelper(typeof(FileCabinetRecord));

            this.firstNameSearchDictionary = new Dictionary<string, List<long>>();
            this.lastNameSearchDictionary = new Dictionary<string, List<long>>();
            this.dateOfBirthSearchDictionary = new Dictionary<DateTime, List<long>>();

            this.SetServiceInfo();
        }

        [Flags]
        private enum RecordStatus : short
        {
            IsDeleted = 4,
        }

        /// <inheritdoc cref="IFileCabinetService.CreateRecord(FileCabinetRecordParameterObject)"/>
        public int CreateRecord(FileCabinetRecordParameterObject parameterObject)
        {
            this.recordValidator.ValidateParameters(parameterObject);

            var newRecord = new FileCabinetRecord
            {
                Id = ++this.lastRecordId,
                FirstName = parameterObject.FirstName,
                LastName = parameterObject.LastName,
                DateOfBirth = parameterObject.DateOfBirth,
                Gender = parameterObject.Gender,
                Weight = parameterObject.Weight,
                Stature = parameterObject.Stature,
            };

            this.fileStream.Seek(0, SeekOrigin.End);

            var creationPlaceAddress = this.fileStream.Position;

            this.dumpHelper.Create(this.fileStream, newRecord);

            this.AddEntryToSearchDictionaries(newRecord, creationPlaceAddress);

            return this.lastRecordId;
        }

        /// <inheritdoc cref="IFileCabinetService.EditRecord(FileCabinetRecordParameterObject)"/>
        public void EditRecord(FileCabinetRecordParameterObject parameterObject)
        {
            this.recordValidator.ValidateParameters(parameterObject);

            var recordAddress = this.GetRecordAddressById(parameterObject.Id);
            if (recordAddress < 0)
            {
                throw new ArgumentException($"User with {parameterObject.Id} does not exist!");
            }

            this.fileStream.Seek(recordAddress, SeekOrigin.Begin);
            var record = (FileCabinetRecord)this.dumpHelper.Read(this.fileStream);

            this.DeleteEntryFromSearchDictionaries(record, recordAddress);

            record.Id = parameterObject.Id;
            record.FirstName = parameterObject.FirstName;
            record.LastName = parameterObject.LastName;
            record.DateOfBirth = parameterObject.DateOfBirth;
            record.Gender = parameterObject.Gender;
            record.Weight = parameterObject.Weight;
            record.Stature = parameterObject.Stature;

            this.AddEntryToSearchDictionaries(record, recordAddress);

            this.fileStream.Seek(recordAddress, SeekOrigin.Begin);
            this.dumpHelper.Update(this.fileStream, record);
        }

        /// <inheritdoc cref="IFileCabinetService.FindByDateOfBirth(DateTime)"/>
        public IRecordIterator FindByDateOfBirth(DateTime dateOfBirth)
        {
            if (!this.dateOfBirthSearchDictionary.ContainsKey(dateOfBirth))
            {
                return new FilesystemIterator(this.fileStream, Array.Empty<long>());
            }

            return new FilesystemIterator(this.fileStream, this.dateOfBirthSearchDictionary[dateOfBirth]);
        }

        /// <inheritdoc cref="IFileCabinetService.FindByFirstName(string)"/>
        public IRecordIterator FindByFirstName(string firstName)
        {
            if (!this.firstNameSearchDictionary.ContainsKey(firstName))
            {
                return new FilesystemIterator(this.fileStream, Array.Empty<long>());
            }

            return new FilesystemIterator(this.fileStream, this.firstNameSearchDictionary[firstName]);
        }

        /// <inheritdoc cref="IFileCabinetService.FindByLastName(string)"/>
        public IRecordIterator FindByLastName(string lastName)
        {
            if (!this.lastNameSearchDictionary.ContainsKey(lastName))
            {
                return new FilesystemIterator(this.fileStream, Array.Empty<long>());
            }

            return new FilesystemIterator(this.fileStream, this.lastNameSearchDictionary[lastName]);
        }

        /// <inheritdoc cref="IFileCabinetService.GetRecords"/>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);

            var reservedAreaSize = this.dumpHelper.GetSize("Reserved");

            var result = new List<FileCabinetRecord>();
            for (int i = 0; i < this.fileStream.Length; i += this.dumpHelper.SliceSize)
            {
                var recordStatus = (RecordStatus)StreamHelper.ReadShort(this.fileStream);
                if ((recordStatus & RecordStatus.IsDeleted) == RecordStatus.IsDeleted)
                {
                    this.fileStream.Seek(this.dumpHelper.SliceSize - reservedAreaSize, SeekOrigin.Current);
                    continue;
                }

                this.fileStream.Seek(-reservedAreaSize, SeekOrigin.Current);

                var record = this.dumpHelper.Read(this.fileStream);
                if (record is null)
                {
                    throw new InvalidDataException($"Record with number {i / this.dumpHelper.SliceSize} can't be read.");
                }

                result.Add((FileCabinetRecord)record);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }

        /// <inheritdoc cref="IFileCabinetService.GetStat"/>
        public (int total, int deleted) GetStat()
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);

            var reservedAreaSize = this.dumpHelper.GetSize("Reserved");

            var recordsCount = 0;
            var deletedRecordsCount = 0;
            for (int i = 0; i < this.fileStream.Length; i += this.dumpHelper.SliceSize)
            {
                var reserved = (RecordStatus)StreamHelper.ReadShort(this.fileStream);
                if ((reserved & RecordStatus.IsDeleted) == RecordStatus.IsDeleted)
                {
                    deletedRecordsCount++;
                }

                recordsCount++;
                this.fileStream.Seek(this.dumpHelper.SliceSize - reservedAreaSize, SeekOrigin.Current);
            }

            return (recordsCount, deletedRecordsCount);
        }

        /// <inheritdoc cref="IFileCabinetService.RecordExists(int)"/>
        public bool RecordExists(int id)
        {
            Guard.ArgumentGreaterThan(id, 0);

            return this.GetRecordAddressById(id) >= 0;
        }

        /// <inheritdoc cref="IFileCabinetService.Restore(FileCabinetServiceSnapshot, Action{FileCabinetRecord, string})"/>
        public int Restore(FileCabinetServiceSnapshot snapshot, Action<FileCabinetRecord, string> onInvalidRecordImported)
        {
            Guard.ArgumentIsNotNull(snapshot, nameof(snapshot));

            var maxId = this.lastRecordId;

            var importedRecords = 0;
            foreach (var record in snapshot.Records)
            {
                try
                {
                    this.recordValidator.Validate(record);
                }
                catch (ArgumentException exception)
                {
                    onInvalidRecordImported?.Invoke(record, exception.Message);
                    continue;
                }

                if (maxId < record.Id)
                {
                    maxId = record.Id;
                }

                var recordAddress = this.GetRecordAddressById(record.Id);
                if (recordAddress < 0)
                {
                    this.fileStream.Seek(0, SeekOrigin.End);

                    this.AddEntryToSearchDictionaries(record, this.fileStream.Position);
                }
                else
                {
                    this.fileStream.Seek(recordAddress, SeekOrigin.Begin);

                    var replacableRecord = (FileCabinetRecord)this.dumpHelper.Read(this.fileStream);
                    this.DeleteEntryFromSearchDictionaries(replacableRecord, recordAddress);
                    this.AddEntryToSearchDictionaries(record, recordAddress);

                    this.fileStream.Seek(recordAddress, SeekOrigin.Begin);
                }

                this.dumpHelper.Create(this.fileStream, record);

                importedRecords++;
            }

            this.lastRecordId = maxId;

            return importedRecords;
        }

        /// <inheritdoc cref="IFileCabinetService.RemoveRecord(int)"/>
        public void RemoveRecord(int recordId)
        {
            Guard.ArgumentGreaterThan(recordId, 0, $"{nameof(recordId)} must be greater than 0.");

            var recordAddress = this.GetRecordAddressById(recordId);
            if (recordAddress < 0)
            {
                throw new ArgumentException($"Record #{recordId} does not exist.", nameof(recordId));
            }

            this.fileStream.Seek(recordAddress, SeekOrigin.Begin);

            var record = (FileCabinetRecord)this.dumpHelper.Read(this.fileStream);

            this.DeleteEntryFromSearchDictionaries(record, recordAddress);

            this.fileStream.Seek(recordAddress, SeekOrigin.Begin);

            var flags = (short)RecordStatus.IsDeleted;
            StreamHelper.Write(this.fileStream, flags);

            this.fileStream.Flush();
        }

        /// <inheritdoc cref="IFileCabinetService.Purge"/>
        public void Purge()
        {
            this.ClearSearchDictionaries();

            this.fileStream.Seek(0, SeekOrigin.Begin);

            var reservedAreaSize = this.dumpHelper.GetSize("Reserved");
            var firstNameAreaSize = this.dumpHelper.GetSize("FirstName");
            var lastNameAreaSize = this.dumpHelper.GetSize("LastName");
            var dateOfBirthAreaSize = this.dumpHelper.GetSize("DateOfBirth");

            var firstNameOffset = this.dumpHelper.GetOffset("FirstName");

            var insertRecordAddress = 0L;
            long currentRecordAddress;

            for (currentRecordAddress = 0; currentRecordAddress < this.fileStream.Length; currentRecordAddress += this.dumpHelper.SliceSize)
            {
                var reserved = (RecordStatus)StreamHelper.ReadShort(this.fileStream);
                if ((reserved & RecordStatus.IsDeleted) == RecordStatus.IsDeleted)
                {
                    insertRecordAddress = currentRecordAddress;
                    this.fileStream.Seek(this.dumpHelper.SliceSize - reservedAreaSize, SeekOrigin.Current);
                    break;
                }

                this.fileStream.Seek(-reservedAreaSize + firstNameOffset, SeekOrigin.Current);

                var firstName = StreamHelper.ReadString(this.fileStream, firstNameAreaSize);
                var lastName = StreamHelper.ReadString(this.fileStream, lastNameAreaSize);
                var dateOfBirth = StreamHelper.ReadDateTime(this.fileStream);

                this.AddEntryToSearchDictionaries(firstName, lastName, dateOfBirth, currentRecordAddress);

                this.fileStream.Seek(currentRecordAddress + this.dumpHelper.SliceSize, SeekOrigin.Begin);
            }

            if (currentRecordAddress >= this.fileStream.Length)
            {
                return;
            }

            var buffer = new byte[this.dumpHelper.SliceSize];
            for (currentRecordAddress = currentRecordAddress + this.dumpHelper.SliceSize; currentRecordAddress < this.fileStream.Length; currentRecordAddress += this.dumpHelper.SliceSize)
            {
                var reservedBytes = StreamHelper.ReadShort(this.fileStream);
                if (((RecordStatus)reservedBytes & RecordStatus.IsDeleted) != RecordStatus.IsDeleted)
                {
                    this.fileStream.Seek(currentRecordAddress + firstNameOffset, SeekOrigin.Begin);

                    var firstName = StreamHelper.ReadString(this.fileStream, firstNameAreaSize);
                    var lastName = StreamHelper.ReadString(this.fileStream, lastNameAreaSize);
                    var dateOfBirth = StreamHelper.ReadDateTime(this.fileStream);

                    this.AddEntryToSearchDictionaries(firstName, lastName, dateOfBirth, insertRecordAddress);

                    this.fileStream.Seek(currentRecordAddress, SeekOrigin.Begin);
                    this.fileStream.Read(buffer);

                    this.fileStream.Seek(insertRecordAddress, SeekOrigin.Begin);
                    this.fileStream.Write(buffer);

                    insertRecordAddress += this.dumpHelper.SliceSize;
                }

                this.fileStream.Seek(currentRecordAddress + this.dumpHelper.SliceSize, SeekOrigin.Begin);
            }

            this.fileStream.Flush();

            this.fileStream.SetLength(insertRecordAddress);
        }

        private int GetRecordAddressById(int id)
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);

            var readBytesSize = this.dumpHelper.GetSize("Id") + this.dumpHelper.GetSize("Reserved");
            for (int currentAddress = 0; currentAddress < this.fileStream.Length; currentAddress += this.dumpHelper.SliceSize)
            {
                var reserved = (RecordStatus)StreamHelper.ReadShort(this.fileStream);
                var recordId = StreamHelper.ReadInt(this.fileStream);

                if ((reserved & RecordStatus.IsDeleted) != RecordStatus.IsDeleted && recordId == id)
                {
                    return currentAddress;
                }

                this.fileStream.Seek(this.dumpHelper.SliceSize - readBytesSize, SeekOrigin.Current);
            }

            return -1;
        }

        private void SetServiceInfo()
        {
            this.firstNameSearchDictionary.Clear();
            this.lastNameSearchDictionary.Clear();
            this.dateOfBirthSearchDictionary.Clear();

            if (this.fileStream.Length == 0)
            {
                this.lastRecordId = 0;
                return;
            }

            var reservedAreaSize = this.dumpHelper.GetSize("Reserved");

            this.fileStream.Seek(0, SeekOrigin.Begin);

            var maxId = 1;
            for (var i = 0; i < this.fileStream.Length; i += this.dumpHelper.SliceSize)
            {
                var recordStatus = (RecordStatus)StreamHelper.ReadShort(this.fileStream);
                if ((recordStatus & RecordStatus.IsDeleted) != RecordStatus.IsDeleted)
                {
                    this.fileStream.Seek(-reservedAreaSize, SeekOrigin.Current);

                    var readRecord = (FileCabinetRecord)this.dumpHelper.Read(this.fileStream);

                    if (maxId < readRecord.Id)
                    {
                        maxId = readRecord.Id;
                    }

                    this.AddEntryToSearchDictionaries(readRecord, i);
                }
                else
                {
                    this.fileStream.Seek(this.dumpHelper.SliceSize - reservedAreaSize, SeekOrigin.Current);
                }
            }

            this.lastRecordId = maxId;
        }

        private void ClearSearchDictionaries()
        {
            this.firstNameSearchDictionary.Clear();
            this.lastNameSearchDictionary.Clear();
            this.dateOfBirthSearchDictionary.Clear();
        }

        private void AddEntryToSearchDictionaries(FileCabinetRecord readRecord, long recordAddress)
        {
            this.AddEntryToSearchDictionaries(readRecord.FirstName, readRecord.LastName, readRecord.DateOfBirth, recordAddress);
        }

        private void AddEntryToSearchDictionaries(string firstName, string lastName, DateTime dateOfBirth, long recordAddress)
        {
            this.AddSearchEntry(this.firstNameSearchDictionary, firstName, recordAddress);
            this.AddSearchEntry(this.lastNameSearchDictionary, lastName, recordAddress);
            this.AddSearchEntry(this.dateOfBirthSearchDictionary, dateOfBirth, recordAddress);
        }

        private void DeleteEntryFromSearchDictionaries(FileCabinetRecord readRecord, long recordAddress)
        {
            this.DeleteEntryFromSearchDictionaries(readRecord.FirstName, readRecord.LastName, readRecord.DateOfBirth, recordAddress);
        }

        private void DeleteEntryFromSearchDictionaries(string firstName, string lastName, DateTime dateOfBirth, long recordAddress)
        {
            this.DeleteSearchEntry(this.firstNameSearchDictionary, firstName, recordAddress);
            this.DeleteSearchEntry(this.lastNameSearchDictionary, lastName, recordAddress);
            this.DeleteSearchEntry(this.dateOfBirthSearchDictionary, dateOfBirth, recordAddress);
        }

        private void AddSearchEntry<TKey, TValue>(Dictionary<TKey, List<TValue>> searchDictionary, TKey searchKey, TValue address)
            where TKey : notnull
        {
            if (!searchDictionary.ContainsKey(searchKey))
            {
                searchDictionary.Add(searchKey, new List<TValue>());
            }

            searchDictionary[searchKey].Add(address);
        }

        private void DeleteSearchEntry<TKey, TValue>(Dictionary<TKey, List<TValue>> searchDictionary, TKey searchKey, TValue address)
            where TKey : notnull
        {
            if (searchDictionary.ContainsKey(searchKey))
            {
                searchDictionary[searchKey].Remove(address);
            }
        }
    }
}
