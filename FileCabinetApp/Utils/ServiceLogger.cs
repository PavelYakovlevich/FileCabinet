using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using FileCabinetApp.Services;

namespace FileCabinetApp.Utils
{
    public class ServiceLogger : IFileCabinetService
    {
        private static readonly string DateStringFormat = "MM/dd/yyyy HH:mm";

        private Stream stream;
        private IFileCabinetService service;

        public ServiceLogger(Stream stream, IFileCabinetService service)
        {
            Guard.ArgumentIsNotNull(stream, nameof(stream));
            Guard.ArgumentIsNotNull(service, nameof(service));

            this.stream = stream;
            this.service = service;
        }

        public int CreateRecord(FileCabinetRecordParameterObject parameterObject)
        {
            this.stream.Seek(0, SeekOrigin.End);

            using (var streamWriter = new StreamWriter(this.stream, Encoding.Default, -1, true))
            {
                var result = this.service.CreateRecord(parameterObject);

                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} Calling CreateRecord() with {this.GetParameterValuesString(parameterObject)}");
                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} CreateRecord() returned '{result}'");

                return result;
            }
        }

        public void EditRecord(FileCabinetRecordParameterObject parameterObject)
        {
            this.stream.Seek(0, SeekOrigin.End);

            using (var streamWriter = new StreamWriter(this.stream, Encoding.Default, -1, true))
            {
                this.service.EditRecord(parameterObject);

                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} Calling EditRecord() with {this.GetParameterValuesString(parameterObject)}");
            }
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            this.stream.Seek(0, SeekOrigin.End);

            using (var streamWriter = new StreamWriter(this.stream, Encoding.Default, -1, true))
            {
                var result = this.service.FindByDateOfBirth(dateOfBirth);

                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} Calling FindByDateOfBirth() with DateOfBirth = '{dateOfBirth.ToString("MM/dd/yyyy")}'");
                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} FindByDateOfBirth() returned '{result.Count}' records.");

                return result;
            }
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.stream.Seek(0, SeekOrigin.End);

            using (var streamWriter = new StreamWriter(this.stream, Encoding.Default, -1, true))
            {
                var result = this.service.FindByFirstName(firstName);

                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} Calling FindByFirstName() with FirstName = '{firstName}'");
                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} FindByFirstName() returned '{result.Count}' records.");

                return result;
            }
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.stream.Seek(0, SeekOrigin.End);

            using (var streamWriter = new StreamWriter(this.stream, Encoding.Default, -1, true))
            {
                var result = this.service.FindByLastName(lastName);

                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} Calling FindByLastName() with LastName = '{lastName}'");
                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} FindByLastName() returned '{result.Count}' records.");

                return result;
            }
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.stream.Seek(0, SeekOrigin.End);

            using (var streamWriter = new StreamWriter(this.stream, Encoding.Default, -1, true))
            {
                var result = this.service.GetRecords();

                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} Calling GetRecords()");
                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} GetRecords() returned '{result.Count}' records.");

                return result;
            }
        }

        public (int total, int deleted) GetStat()
        {
            this.stream.Seek(0, SeekOrigin.End);

            using (var streamWriter = new StreamWriter(this.stream, Encoding.Default, -1, true))
            {
                var result = this.service.GetStat();

                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} Calling GetStat()");
                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} GetStat() returned '{result.total}' total and '{result.deleted}' deleted records.");

                return result;
            }
        }

        public bool RecordExists(int id)
        {
            this.stream.Seek(0, SeekOrigin.End);

            using (var streamWriter = new StreamWriter(this.stream, Encoding.Default, -1, true))
            {
                var result = this.service.RecordExists(id);

                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} Calling RecordExists() with id = '{id}'");
                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} RecordExists() returned '{result}'.");

                return result;
            }
        }

        public void RemoveRecord(int recordId)
        {
            this.stream.Seek(0, SeekOrigin.End);

            using (var streamWriter = new StreamWriter(this.stream, Encoding.Default, -1, true))
            {
                this.service.RemoveRecord(recordId);

                streamWriter.WriteLine($"{DateTime.Now.ToString(DateStringFormat)} Calling RemoveRecord() with id = '{recordId}'");
            }
        }

        public int Restore(FileCabinetServiceSnapshot snapshot, Action<FileCabinetRecord, string> onInvalidRecordImported)
        {
            this.stream.Seek(0, SeekOrigin.End);

            using (var streamWriter = new StreamWriter(this.stream, Encoding.Default, -1, true))
            {
                var result = this.service.Restore(snapshot, onInvalidRecordImported);

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
