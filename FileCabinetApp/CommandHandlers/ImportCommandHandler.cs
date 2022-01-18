using System;
using System.IO;

using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling of 'import' command.
    /// </summary>
    public class ImportCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ImportCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service for working with file cabinet records.</param>
        public ImportCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        ///     Handles 'import' command.
        /// </summary>
        /// <param name="commandRequest">Input data for a requested command.</param>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (!commandRequest.Command.Equals("import", StringComparison.InvariantCultureIgnoreCase))
            {
                base.Handle(commandRequest);
                return;
            }

            var parametersValues = commandRequest.Parameters.Split(' ');

            if (parametersValues.Length < 2)
            {
                Console.WriteLine("Missing import file format or file path.");
                return;
            }

            var importFormat = parametersValues[0].Trim();
            if (!importFormat.Equals("csv", StringComparison.InvariantCultureIgnoreCase) && !importFormat.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine($"Import error: Unsupported import format: {importFormat}");
                return;
            }

            var importFilePath = parametersValues[1].Trim();
            if (importFilePath.Length == 0)
            {
                Console.WriteLine($"Import error: import file was not specified.");
                return;
            }

            if (!File.Exists(importFilePath))
            {
                Console.WriteLine($"Import error: file {importFilePath} is not exist.");
                return;
            }

            try
            {
                int importedRecordsCount = 0;

                using (var stream = new FileStream(importFilePath, FileMode.Open))
                {
                    var snapshot = new FileCabinetServiceSnapshot();

                    using (var streamReader = new StreamReader(stream))
                    {
                        switch (importFormat)
                        {
                            case "csv":
                                snapshot.LoadFromCsv(streamReader);
                                break;
                            case "xml":
                                snapshot.LoadFromXml(streamReader);
                                break;
                            default:
                                Console.WriteLine($"Import error: format: {importFormat} is not defined.");
                                return;
                        }
                    }

                    importedRecordsCount = this.service.Restore(snapshot, (record, message) => Console.WriteLine($"Import of record with id : {record.Id} failed with error: {message}"));
                }

                Console.WriteLine($"{importedRecordsCount} records were imported from {importFilePath}.");
            }
            catch (IOException exception)
            {
                Console.WriteLine($"Import failed: {exception.Message}");
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                Console.WriteLine($"Access error: {unauthorizedAccessException.Message}");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Oops, something went wrong: {exception.InnerException?.Message}.");
            }
        }
    }
}