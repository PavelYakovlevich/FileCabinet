using System;
using System.Collections.ObjectModel;
using System.IO;

namespace FileCabinetApp.Services
{
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private IRecordValidator recordValidator;
        private Stream fileStream;

        public FileCabinetFilesystemService(IRecordValidator recordValidator, Stream stream)
        {
            Guard.ArgumentIsNotNull(recordValidator, nameof(recordValidator));
            Guard.ArgumentIsNotNull(stream, nameof(stream));

            this.recordValidator = recordValidator;
            this.fileStream = stream;
        }

        public int CreateRecord(FileCabinetRecordParameterObject parameterObject)
        {
            throw new NotImplementedException();
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
    }
}
