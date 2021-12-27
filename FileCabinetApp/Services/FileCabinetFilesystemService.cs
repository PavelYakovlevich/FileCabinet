using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Services
{
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private IRecordValidator recordValidator;
        private FileStream fileStream;

        public FileCabinetFilesystemService(IRecordValidator recordValidator)
        {
            this.recordValidator = recordValidator;
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
