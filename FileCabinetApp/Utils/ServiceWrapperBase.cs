using System;
using System.Collections.ObjectModel;
using FileCabinetApp.Services;

namespace FileCabinetApp.Utils
{
    public abstract class ServiceWrapperBase : IFileCabinetService
    {
        protected readonly IFileCabinetService service;

        protected ServiceWrapperBase(IFileCabinetService service)
        {
            Guard.ArgumentIsNotNull(service, nameof(service));

            this.service = service;
        }

        public abstract int CreateRecord(FileCabinetRecordParameterObject parameterObject);

        public abstract void EditRecord(FileCabinetRecordParameterObject parameterObject);

        public abstract ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth);

        public abstract ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        public abstract ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        public abstract ReadOnlyCollection<FileCabinetRecord> GetRecords();

        public abstract (int total, int deleted) GetStat();

        public abstract void Purge();

        public abstract bool RecordExists(int id);

        public abstract void RemoveRecord(int recordId);

        public abstract int Restore(FileCabinetServiceSnapshot snapshot, Action<FileCabinetRecord, string> onInvalidRecordImported);

        public IFileCabinetService GetWrappedObject()
        {

            if (this.service is ServiceWrapperBase)
            {
                return ((ServiceWrapperBase)this.service).service;
            }

            return this.service;
        }
    }
}
