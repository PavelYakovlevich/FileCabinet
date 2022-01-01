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

            var result = new List<FileCabinetRecord>();
            var recordsCount = this.fileStream.Length / this.dumpHelper.SliceSize;
            for (int i = 0; i < recordsCount; i++)
            {
                var record = this.dumpHelper.Read(this.fileStream);

                if (record is null)
                {
                    throw new InvalidDataException($"Record with number {i} can't be read.");
                }

                result.Add((FileCabinetRecord)record);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }

        /// <inheritdoc cref="IFileCabinetService.GetStat"/>
        public int GetStat()
        {
            var recordsCount = 0;

            for (int i = 0; i < this.fileStream.Length; i += this.dumpHelper.SliceSize)
            {
                recordsCount++;
            }

            return recordsCount;
        }

        /// <inheritdoc cref="IFileCabinetService.RecordExists(int)"/>
        public bool RecordExists(int id)
        {
            Guard.ArgumentGreaterThan(id, 0);

            return this.GetRecordAddressById(id) >= 0;
        }

        private int GetRecordAddressById(int id)
        {
            var recordIdOffset = this.dumpHelper.GetOffset("Id");
            var sizeOfId = this.dumpHelper.GetSize("Id");

            this.fileStream.Seek(recordIdOffset, SeekOrigin.Begin);
            for (int i = recordIdOffset; i < this.fileStream.Length; i += this.dumpHelper.SliceSize)
            {
                var recordId = StreamHelper.ReadInt(this.fileStream);

                if (id == recordId)
                {
                    return i - recordIdOffset;
                }

                this.fileStream.Seek(this.dumpHelper.SliceSize - sizeOfId, SeekOrigin.Current);
            }

            return -1;
        }

        private void SetLastRecordId()
        {
            var recordIdOffset = this.dumpHelper.GetOffset("Id");

            this.fileStream.Seek(-this.dumpHelper.SliceSize + recordIdOffset, SeekOrigin.End);

            this.lastRecordId = StreamHelper.ReadInt(this.fileStream);
        }

        private ReadOnlyCollection<FileCabinetRecord> FindByCondition(Predicate<FileCabinetRecord> condition)
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);
            var result = new List<FileCabinetRecord>();
            for (int i = 0; i < this.fileStream.Length; i += this.dumpHelper.SliceSize)
            {
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
