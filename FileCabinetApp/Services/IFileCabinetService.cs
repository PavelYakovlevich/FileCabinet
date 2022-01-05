using System;
using System.Collections.ObjectModel;

namespace FileCabinetApp.Services
{
    /// <summary>
    ///     Interface for the file cabinet services.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        ///     Creates a file cabinet record with the specified property's values.
        /// </summary>
        /// <param name="parameterObject.">Parameter object, which holds all necessary data for the creation of <see cref="FileCabinetRecord"/> object.</param>
        /// <returns>Id of new file cabinet record.</returns>
        int CreateRecord(FileCabinetRecordParameterObject parameterObject);

        /// <summary>
        ///     Edits a file cabinet record with the specified property's values.
        /// </summary>
        /// <param name="parameterObject.">Parameter object, which holds all necessary data for the editting of <see cref="FileCabinetRecord"/> object.</param>
        void EditRecord(FileCabinetRecordParameterObject parameterObject);

        /// <summary>
        ///     Gets count of all existing file cabinet records.
        /// </summary>
        /// <returns>Count of all existing file cabinet records.</returns>
        int GetStat();

        /// <summary>
        ///     Checks if file cabinet record with specified <paramref name="id"/> exists.
        /// </summary>
        /// <param name="id">Id of the file cabinet record.</param>
        /// <returns>True if file cabinet record with specified id exists, overwise false.</returns>
        bool RecordExists(int id);

        /// <summary>
        ///     Gets array of all existing <see cref="FileCabinetRecord"/>.
        /// </summary>
        /// <returns>Array of all existing <see cref="FileCabinetRecord"/>.</returns>
        ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        ///     Perfoms search of all existing <see cref="FileCabinetRecord"/> that have first name's value equal to the specified <paramref name="firstName"/>.
        /// </summary>
        /// <param name="firstName">First name search value.</param>
        /// <returns>All <see cref="FileCabinetRecord"/> records, which have the same first name value as <paramref name="firstName"/>.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        ///     Perfoms search of all existing <see cref="FileCabinetRecord"/> that have last name's value equal to the specified <paramref name="lastName"/>.
        /// </summary>
        /// <param name="lastName">Last name search value.</param>
        /// <returns>All <see cref="FileCabinetRecord"/> records, which have the same last name value as <paramref name="lastName"/>.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        ///     Perfoms search of all existing <see cref="FileCabinetRecord"/> that have date of birth's value equal to the specified <paramref name="dateOfBirth"/>.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth search value.</param>
        /// <returns>All <see cref="FileCabinetRecord"/> records, which have the same birthday value as <paramref name="dateOfBirth"/>.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth);

        /// <summary>
        ///     Restores state of service from a snapshot.
        /// </summary>
        /// <param name="snapshot"><see cref="FileCabinetServiceSnapshot"/> object, which holds a state of the service.</param>
        /// <param name="onInvalidRecordImported">Action, which is called after an invalid record was imported.</param>
        /// <returns>Restored <see cref="FileCabinetRecord"/>'s count.</returns>
        public int Restore(FileCabinetServiceSnapshot snapshot, Action<FileCabinetRecord, string> onInvalidRecordImported);
    }
}
