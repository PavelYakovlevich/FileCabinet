using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using FileCabinetApp.Services;

namespace FileCabinetApp.Utils
{
    public class ServiceMeter : IFileCabinetService
    {
        private static readonly string PrintFormatStr = "{0} method execution duration is {1} ticks.";

        private readonly IFileCabinetService service;

        public ServiceMeter(IFileCabinetService service)
        {
            Guard.ArgumentIsNotNull(service, nameof(service));

            this.service = service;
        }

        public int CreateRecord(FileCabinetRecordParameterObject parameterObject)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var result = this.service.CreateRecord(parameterObject);

            stopwatch.Stop();

            this.PrintMeasurementResult(stopwatch.ElapsedTicks);

            return result;
        }

        public void EditRecord(FileCabinetRecordParameterObject parameterObject)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            this.service.EditRecord(parameterObject);

            stopwatch.Stop();

            this.PrintMeasurementResult(stopwatch.ElapsedTicks);
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var result = this.service.FindByDateOfBirth(dateOfBirth);

            stopwatch.Stop();

            this.PrintMeasurementResult(stopwatch.ElapsedTicks);

            return result;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var result = this.service.FindByFirstName(firstName);

            stopwatch.Stop();

            this.PrintMeasurementResult(stopwatch.ElapsedTicks);

            return result;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var result = this.service.FindByLastName(lastName);

            stopwatch.Stop();

            this.PrintMeasurementResult(stopwatch.ElapsedTicks);

            return result;
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var result = this.service.GetRecords();

            stopwatch.Stop();

            this.PrintMeasurementResult(stopwatch.ElapsedTicks);

            return result;
        }

        public (int total, int deleted) GetStat()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var result = this.service.GetStat();

            stopwatch.Stop();

            this.PrintMeasurementResult(stopwatch.ElapsedTicks);

            return result;
        }

        public void Purge()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            this.service.Purge();

            stopwatch.Stop();

            this.PrintMeasurementResult(stopwatch.ElapsedTicks);
        }

        public bool RecordExists(int id)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var result = this.service.RecordExists(id);

            stopwatch.Stop();

            this.PrintMeasurementResult(stopwatch.ElapsedTicks);

            return result;
        }

        public void RemoveRecord(int recordId)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            this.service.RemoveRecord(recordId);

            stopwatch.Stop();

            this.PrintMeasurementResult(stopwatch.ElapsedTicks);
        }

        public int Restore(FileCabinetServiceSnapshot snapshot, Action<FileCabinetRecord, string> onInvalidRecordImported)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var result = this.service.Restore(snapshot, onInvalidRecordImported);

            stopwatch.Stop();

            this.PrintMeasurementResult(stopwatch.ElapsedTicks);

            return result;
        }

        private void PrintMeasurementResult(long ticks, [CallerMemberName] string methodName = "")
        {
            Console.WriteLine(PrintFormatStr, methodName, ticks);
        }
    }
}
