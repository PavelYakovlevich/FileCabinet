using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

            this.SetLastRecordId();
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

            this.dumpHelper.Create(this.fileStream, newRecord);

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

            var newRecord = new FileCabinetRecord
            {
                Id = parameterObject.Id,
                FirstName = parameterObject.FirstName,
                LastName = parameterObject.LastName,
                DateOfBirth = parameterObject.DateOfBirth,
                Gender = parameterObject.Gender,
                Weight = parameterObject.Weight,
                Stature = parameterObject.Stature,
            };

            this.fileStream.Seek(recordAddress, SeekOrigin.Begin);
            this.dumpHelper.Update(this.fileStream, newRecord);
        }

        /// <inheritdoc cref="IFileCabinetService.FindByDateOfBirth(DateTime)"/>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            return this.FindByCondition((record) => record.DateOfBirth.Equals(dateOfBirth));
        }

        /// <inheritdoc cref="IFileCabinetService.FindByFirstName(string)"/>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            return this.FindByCondition((record) => record.FirstName.Equals(firstName, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <inheritdoc cref="IFileCabinetService.FindByLastName(string)"/>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            return this.FindByCondition((record) => record.LastName.Equals(lastName, StringComparison.InvariantCultureIgnoreCase));
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

                var recordAddress = this.GetRecordAddressById(record.Id);
                if (recordAddress < 0)
                {
                    this.fileStream.Seek(0, SeekOrigin.End);
                }
                else
                {
                    this.fileStream.Seek(recordAddress, SeekOrigin.Begin);
                }

                this.dumpHelper.Create(this.fileStream, record);

                importedRecords++;
            }

            this.SetLastRecordId();

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

            var flags = (short)RecordStatus.IsDeleted;
            StreamHelper.Write(this.fileStream, flags);

            this.fileStream.Flush();
        }

        /// <inheritdoc cref="IFileCabinetService.Purge"/>
        public void Purge()
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);

            var reservedAreaSize = this.dumpHelper.GetSize("Reserved");

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

                this.fileStream.Seek(this.dumpHelper.SliceSize - reservedAreaSize, SeekOrigin.Current);
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
                    var convertedReservedArea = BitConverter.GetBytes(reservedBytes);
                    Array.Copy(convertedReservedArea, buffer, convertedReservedArea.Length);

                    this.fileStream.Read(buffer, reservedAreaSize, this.dumpHelper.SliceSize - reservedAreaSize);

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

        private void SetLastRecordId()
        {
            var reservedAreaSize = this.dumpHelper.GetSize("Reserved");

            if (this.fileStream.Length == 0)
            {
                this.lastRecordId = 0;
                return;
            }

            this.fileStream.Seek(-this.dumpHelper.SliceSize, SeekOrigin.End);

            for (var i = this.fileStream.Length - this.dumpHelper.SliceSize; i >= 0; i -= this.dumpHelper.SliceSize)
            {
                var recordStatus = (RecordStatus)StreamHelper.ReadShort(this.fileStream);
                if ((recordStatus & RecordStatus.IsDeleted) != RecordStatus.IsDeleted)
                {
                    this.lastRecordId = StreamHelper.ReadInt(this.fileStream);
                    return;
                }

                this.fileStream.Seek(-this.dumpHelper.SliceSize - reservedAreaSize, SeekOrigin.Current);
            }
        }

        private ReadOnlyCollection<FileCabinetRecord> FindByCondition(Predicate<FileCabinetRecord> condition)
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

                var currentRecord = (FileCabinetRecord)this.dumpHelper.Read(this.fileStream) !;
                if (condition(currentRecord))
                {
                    result.Add(currentRecord);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }
    }
}
