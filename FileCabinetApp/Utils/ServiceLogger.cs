using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using FileCabinetApp.Services;

namespace FileCabinetApp.Utils
{
    /// <summary>
    ///     Logger wrapper for the <see cref="IFileCabinetService"/> objects.
    /// </summary>
    public class ServiceLogger : ServiceWrapperBase
    {
        private static readonly string DateStringFormat = "MM/dd/yyyy HH:mm";

        private readonly Stream stream;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ServiceLogger"/> class.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object, to which logs will be written.</param>
        /// <param name="service">Wrappable <see cref="IFileCabinetService"/> object.</param>
        public ServiceLogger(Stream stream, IFileCabinetService service)
            : base(service)
        {
            Guard.ArgumentIsNotNull(stream, nameof(stream));

            this.stream = stream;
        }

        /// <summary>
        ///     Writes a log with service information about CreateRecord command.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.CreateRecord(FileCabinetRecordParameterObject)"/>
        public override int CreateRecord(FileCabinetRecordParameterObject parameterObject)
        {
            this.stream.Seek(0, SeekOrigin.End);

            using (var streamWriter = new StreamWriter(this.stream, Encoding.Default, -1, true))
            {
                var result = this.Service.CreateRecord(parameterObject);

                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} Calling CreateRecord() with {this.GetParameterValuesString(parameterObject)}");
                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} CreateRecord() returned '{result}'");

                return result;
            }
        }

        /// <summary>
        ///     Writes a log with service information about EditRecord command.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.EditRecord(FileCabinetRecordParameterObject)"/>
        public override void EditRecord(FileCabinetRecordParameterObject parameterObject)
        {
            this.stream.Seek(0, SeekOrigin.End);

            using (var streamWriter = new StreamWriter(this.stream, Encoding.Default, -1, true))
            {
                this.Service.EditRecord(parameterObject);

                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} Calling EditRecord() with {this.GetParameterValuesString(parameterObject)}");
            }
        }

        /// <summary>
        ///     Writes a log with service information about FindByDateOfBirth command.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.FindByDateOfBirth(DateTime)"/>
        public override ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            this.stream.Seek(0, SeekOrigin.End);

            using (var streamWriter = new StreamWriter(this.stream, Encoding.Default, -1, true))
            {
                var result = this.Service.FindByDateOfBirth(dateOfBirth);

                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} Calling FindByDateOfBirth() with DateOfBirth = '{dateOfBirth.ToString("MM/dd/yyyy")}'");
                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} FindByDateOfBirth() returned '{result.Count}' records.");

                return result;
            }
        }

        /// <summary>
        ///     Writes a log with service information about FindByFirstName command.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.FindByFirstName(string)"/>
        public override ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.stream.Seek(0, SeekOrigin.End);

            using (var streamWriter = new StreamWriter(this.stream, Encoding.Default, -1, true))
            {
                var result = this.Service.FindByFirstName(firstName);

                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} Calling FindByFirstName() with FirstName = '{firstName}'");
                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} FindByFirstName() returned '{result.Count}' records.");

                return result;
            }
        }

        /// <summary>
        ///     Writes a log with service information about FindByLastName command.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.FindByLastName(string)"/>
        public override ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.stream.Seek(0, SeekOrigin.End);

            using (var streamWriter = new StreamWriter(this.stream, Encoding.Default, -1, true))
            {
                var result = this.Service.FindByLastName(lastName);

                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} Calling FindByLastName() with LastName = '{lastName}'");
                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} FindByLastName() returned '{result.Count}' records.");

                return result;
            }
        }

        /// <summary>
        ///     Writes a log with service information about GetRecords command.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.GetRecords"/>
        public override ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.stream.Seek(0, SeekOrigin.End);

            using (var streamWriter = new StreamWriter(this.stream, Encoding.Default, -1, true))
            {
                var result = this.Service.GetRecords();

                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} Calling GetRecords()");
                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} GetRecords() returned '{result.Count}' records.");

                return result;
            }
        }

        /// <summary>
        ///     Writes a log with service information about GetStat command.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.GetStat"/>
        public override (int total, int deleted) GetStat()
        {
            this.stream.Seek(0, SeekOrigin.End);

            using (var streamWriter = new StreamWriter(this.stream, Encoding.Default, -1, true))
            {
                var result = this.Service.GetStat();

                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} Calling GetStat()");
                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} GetStat() returned '{result.total}' total and '{result.deleted}' deleted records.");

                return result;
            }
        }

        /// <summary>
        ///     Writes a log with service information about Purge command.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.Purge"/>
        public override void Purge()
        {
            this.stream.Seek(0, SeekOrigin.End);

            using (var streamWriter = new StreamWriter(this.stream, Encoding.Default, -1, true))
            {
                this.Service.Purge();

                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} Calling Purge()'");
            }
        }

        /// <summary>
        ///     Writes a log with service information about RecordExists command.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.RecordExists(int)"/>
        public override bool RecordExists(int id)
        {
            this.stream.Seek(0, SeekOrigin.End);

            using (var streamWriter = new StreamWriter(this.stream, Encoding.Default, -1, true))
            {
                var result = this.Service.RecordExists(id);

                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} Calling RecordExists() with id = '{id}'");
                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} RecordExists() returned '{result}'.");

                return result;
            }
        }

        /// <summary>
        ///     Writes a log with service information about RemoveRecord command.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.RemoveRecord(int)"/>
        public override void RemoveRecord(int recordId)
        {
            this.stream.Seek(0, SeekOrigin.End);

            using (var streamWriter = new StreamWriter(this.stream, Encoding.Default, -1, true))
            {
                this.Service.RemoveRecord(recordId);

                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} Calling RemoveRecord() with id = '{recordId}'");
            }
        }

        /// <summary>
        ///     Writes a log with service information about Restore command.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.Restore(FileCabinetServiceSnapshot, Action{FileCabinetRecord, string})"/>
        public override int Restore(FileCabinetServiceSnapshot snapshot, Action<FileCabinetRecord, string> onInvalidRecordImported)
        {
            this.stream.Seek(0, SeekOrigin.End);

            using (var streamWriter = new StreamWriter(this.stream, Encoding.Default, -1, true))
            {
                var result = this.Service.Restore(snapshot, onInvalidRecordImported);

                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} Calling Restore()");
                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} Restore() returned '{result}' restored records count.");

                return result;
            }
        }

        private string GetParameterValuesString(FileCabinetRecordParameterObject parameterObject)
        {
            return $"FirstName = '{parameterObject.FirstName}', LastName = '{parameterObject.LastName}', DateOfBirth = '{parameterObject.DateOfBirth}', Gender = '{parameterObject.Gender}', Stature = '{parameterObject.Stature}', Weight = '{parameterObject.Weight}'";
        }
    }
}
