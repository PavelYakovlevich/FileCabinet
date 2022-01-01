using System;
using System.Collections.ObjectModel;
using System.IO;
using FileCabinetApp.Serialization;

namespace FileCabinetApp.Services
{
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private readonly IRecordValidator recordValidator;
        private readonly Stream fileStream;
        private readonly DumpHelper dumpHelper;

        private int lastRecordId;

        public FileCabinetFilesystemService(IRecordValidator recordValidator, Stream stream)
        {
            Guard.ArgumentIsNotNull(recordValidator, nameof(recordValidator));
            Guard.ArgumentIsNotNull(stream, nameof(stream));

            this.recordValidator = recordValidator;
            this.fileStream = stream;
            this.dumpHelper = new DumpHelper(typeof(FileCabinetRecord));

            this.SetLastRecordId();
        }

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

            this.dumpHelper.Write(this.fileStream, newRecord);

            return this.lastRecordId;
        }

        public void EditRecord(FileCabinetRecordParameterObject parameterObject)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

        public int GetStat()
        {
            throw new NotImplementedException();
        }

        public bool RecordExists(int id)
        {
            throw new NotImplementedException();
        }

        private void SetLastRecordId()
        {
            var recordIdOffset = this.dumpHelper.GetOffset("Id");

            this.fileStream.Seek(-this.dumpHelper.SliceSize + recordIdOffset, SeekOrigin.End);

            this.lastRecordId = StreamHelper.ReadInt(this.fileStream);
        }
    }
}
