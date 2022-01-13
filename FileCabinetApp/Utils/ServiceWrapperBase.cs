using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FileCabinetApp.Services;

namespace FileCabinetApp.Utils
{
    /// <summary>
    ///     Abstract base class for all <see cref="IFileCabinetService"/> objects wrappers.
    /// </summary>
    public abstract class ServiceWrapperBase : IFileCabinetService
    {
        private readonly IFileCabinetService service;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ServiceWrapperBase"/> class.
        /// </summary>
        /// <param name="service">Wrappable <see cref="IFileCabinetService"/> object.</param>
        protected ServiceWrapperBase(IFileCabinetService service)
        {
            Guard.ArgumentIsNotNull(service, nameof(service));

            this.service = service;
        }

        /// <summary>
        ///     Gets wrapped service object.
        /// </summary>
        /// <value>Wrapped service object.</value>
        protected IFileCabinetService Service => this.service;

        /// <inheritdoc cref="IFileCabinetService.CreateRecord(FileCabinetRecordParameterObject)"/>
        public abstract int CreateRecord(FileCabinetRecordParameterObject parameterObject);

        /// <inheritdoc cref="IFileCabinetService.EditRecord(FileCabinetRecordParameterObject)"/>
        public abstract void EditRecord(FileCabinetRecordParameterObject parameterObject);

        /// <inheritdoc cref="IFileCabinetService.FindByDateOfBirth(DateTime)"/>
        public abstract IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth);

        /// <inheritdoc cref="IFileCabinetService.FindByFirstName(string)"/>
        public abstract IEnumerable<FileCabinetRecord> FindByFirstName(string firstName);

        /// <inheritdoc cref="IFileCabinetService.FindByLastName(string)"/>
        public abstract IEnumerable<FileCabinetRecord> FindByLastName(string lastName);

        /// <inheritdoc cref="IFileCabinetService.GetRecords"/>
        public abstract ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <inheritdoc cref="IFileCabinetService.GetStat"/>
        public abstract (int total, int deleted) GetStat();

        /// <inheritdoc cref="IFileCabinetService.Purge"/>
        public abstract void Purge();

        /// <inheritdoc cref="IFileCabinetService.RecordExists(int)"/>
        public abstract bool RecordExists(int id);

        /// <inheritdoc cref="IFileCabinetService.RemoveRecord(int)"/>
        public abstract void RemoveRecord(int recordId);

        /// <inheritdoc cref="IFileCabinetService.Restore(FileCabinetServiceSnapshot, Action{FileCabinetRecord, string})"/>
        public abstract int Restore(FileCabinetServiceSnapshot snapshot, Action<FileCabinetRecord, string> onInvalidRecordImported);

        /// <summary>
        ///     Gets initial wrapped object.
        /// </summary>
        /// <returns>Initial wrapped object.</returns>
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
