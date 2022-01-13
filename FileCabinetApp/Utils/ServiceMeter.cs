using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using FileCabinetApp.Services;

namespace FileCabinetApp.Utils
{
    /// <summary>
    ///     Wrapper class for measuring the time of <see cref="IFileCabinetService"/> object's commands execution.
    /// </summary>
    public class ServiceMeter : ServiceWrapperBase
    {
        private static readonly string PrintFormatStr = "{0} method execution duration is {1} ticks.";

        /// <summary>
        ///     Initializes a new instance of the <see cref="ServiceMeter"/> class.
        /// </summary>
        /// <param name="service">Wrappable <see cref="IFileCabinetService"/> object.</param>
        public ServiceMeter(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        ///     Measures and prints CreateRecord's command execution time.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.CreateRecord(FileCabinetRecordParameterObject)"/>
        public override int CreateRecord(FileCabinetRecordParameterObject parameterObject)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var result = this.Service.CreateRecord(parameterObject);

            stopwatch.Stop();

            this.PrintMeasurementResult(stopwatch.ElapsedTicks);

            return result;
        }

        /// <summary>
        ///     Measures and prints EditRecord's command execution time.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.EditRecord(FileCabinetRecordParameterObject)"/>
        public override void EditRecord(FileCabinetRecordParameterObject parameterObject)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            this.Service.EditRecord(parameterObject);

            stopwatch.Stop();

            this.PrintMeasurementResult(stopwatch.ElapsedTicks);
        }

        /// <summary>
        ///     Measures and prints FindByDateOfBirth's command execution time.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.FindByDateOfBirth(DateTime)"/>
        public override ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var result = this.Service.FindByDateOfBirth(dateOfBirth);

            stopwatch.Stop();

            this.PrintMeasurementResult(stopwatch.ElapsedTicks);

            return result;
        }

        /// <summary>
        ///     Measures and prints FindByFirstName's command execution time.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.FindByFirstName(string)"/>
        public override ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var result = this.Service.FindByFirstName(firstName);

            stopwatch.Stop();

            this.PrintMeasurementResult(stopwatch.ElapsedTicks);

            return result;
        }

        /// <summary>
        ///     Measures and prints FindByLastName's command execution time.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.FindByLastName(string)"/>
        public override ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var result = this.Service.FindByLastName(lastName);

            stopwatch.Stop();

            this.PrintMeasurementResult(stopwatch.ElapsedTicks);

            return result;
        }

        /// <summary>
        ///     Measures and prints GetRecords's command execution time.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.GetRecords"/>
        public override ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var result = this.Service.GetRecords();

            stopwatch.Stop();

            this.PrintMeasurementResult(stopwatch.ElapsedTicks);

            return result;
        }

        /// <summary>
        ///     Measures and prints GetStat's command execution time.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.GetStat"/>
        public override (int total, int deleted) GetStat()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var result = this.Service.GetStat();

            stopwatch.Stop();

            this.PrintMeasurementResult(stopwatch.ElapsedTicks);

            return result;
        }

        /// <summary>
        ///     Measures and prints Purge's command execution time.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.Purge"/>
        public override void Purge()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            this.Service.Purge();

            stopwatch.Stop();

            this.PrintMeasurementResult(stopwatch.ElapsedTicks);
        }

        /// <summary>
        ///     Measures and prints RecordExists's command execution time.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.RecordExists(int)"/>
        public override bool RecordExists(int id)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var result = this.Service.RecordExists(id);

            stopwatch.Stop();

            this.PrintMeasurementResult(stopwatch.ElapsedTicks);

            return result;
        }

        /// <summary>
        ///     Measures and prints RemoveRecord's command execution time.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.RemoveRecord(int)"/>
        public override void RemoveRecord(int recordId)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            this.Service.RemoveRecord(recordId);

            stopwatch.Stop();

            this.PrintMeasurementResult(stopwatch.ElapsedTicks);
        }

        /// <summary>
        ///     Measures and prints RemoveRecord's command execution time.
        /// </summary>
        /// <inheritdoc cref="IFileCabinetService.Restore(FileCabinetServiceSnapshot, Action{FileCabinetRecord, string})"/>
        public override int Restore(FileCabinetServiceSnapshot snapshot, Action<FileCabinetRecord, string> onInvalidRecordImported)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var result = this.Service.Restore(snapshot, onInvalidRecordImported);

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
