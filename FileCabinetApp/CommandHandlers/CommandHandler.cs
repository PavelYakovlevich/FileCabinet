using System;
using System.Collections.ObjectModel;
using System.IO;

using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    public class CommandHandler : CommandHandlerBase
    {
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
{
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("export", Export),
            new Tuple<string, Action<string>>("import", Import),
            new Tuple<string, Action<string>>("remove", Remove),
            new Tuple<string, Action<string>>("purge", Purge),
};

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints count of registered users", "The 'stat' command prints count of registered users." },
            new string[] { "create", "creates a new user", "The 'create' command creates a new user." },
            new string[] { "list", "prints the list of all created users", "The 'list' command prints the list of all created users." },
            new string[] { "edit", "edits existing record", "The 'edit' command edits existing record." },
            new string[] { "find", "finds all records with specified criterias", "The 'find' command finds all records with specified criterias." },
            new string[] { "export", "expots all records to the file with specified format", "The 'export' command expots all records to the file with specified format." },
            new string[] { "import", "imports records from the file with specified format", "The 'import' command imports records from the file with specified format." },
            new string[] { "remove", "removes record", "The 'remove' command removes record." },
            new string[] { "purge", "makes defragmentation of the database file", "The 'purge' command makes defragmentation of the database file." },
        };

        /// <inheritdoc cref="CommandHandlerBase.Handle(AppCommandRequest)"/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(commandRequest.Command, StringComparison.InvariantCultureIgnoreCase));
            if (index >= 0)
            {
                commands[index].Item2(commandRequest.Parameters);
            }
            else
            {
                PrintMissedCommandInfo(commandRequest.Command);
            }
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");

            Program.isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsInfo = Program.fileCabinetService.GetStat();
            Console.WriteLine($"Total amount of records: {recordsInfo.total}.");
            Console.WriteLine($"Delete records amount: {recordsInfo.deleted}.");
        }

        private static void Create(string parameters)
        {
            Console.Write("First name: ");
            var firstName = ConsoleParameterReader.ReadInput(InputUtils.StringConverter, Program.consoleInputValidator.ValidateFirstName);

            Console.Write("Last name: ");
            var lastName = ConsoleParameterReader.ReadInput(InputUtils.StringConverter, Program.consoleInputValidator.ValidateLastName);

            Console.Write("Date of birth: ");
            var birthDate = ConsoleParameterReader.ReadInput(InputUtils.DateTimeConverter, Program.consoleInputValidator.ValidateBirthDay);

            Console.Write("Stature: ");
            var stature = ConsoleParameterReader.ReadInput(InputUtils.ShortConverter, Program.consoleInputValidator.ValidateStature);

            Console.Write("Weight: ");
            var weight = ConsoleParameterReader.ReadInput(InputUtils.DecimalConverter, Program.consoleInputValidator.ValidateWeight);

            Console.Write("Gender: ");
            var gender = ConsoleParameterReader.ReadInput(InputUtils.CharConverter, Program.consoleInputValidator.ValidateGender);

            var parameterObject = new FileCabinetRecordParameterObject(firstName, lastName, birthDate, stature, gender, weight);
            var createdRecIndex = Program.fileCabinetService.CreateRecord(parameterObject);

            Console.WriteLine($"Record #{createdRecIndex} is created.");
        }

        private static void List(string parameters)
        {
            var records = Program.fileCabinetService.GetRecords();

            foreach (var record in records)
            {
                Console.WriteLine($"#{record}");
            }
        }

        private static void Edit(string parameters)
        {
            int id;
            if (!int.TryParse(parameters, out id))
            {
                Console.WriteLine("Invalid format of id");
                return;
            }

            if (id <= 0)
            {
                Console.WriteLine("Id must be greater than 0.");
                return;
            }

            if (!Program.fileCabinetService.RecordExists(id))
            {
                Console.WriteLine($"#{id} record is not found.");
                return;
            }

            Console.Write("First name: ");
            var firstName = ConsoleParameterReader.ReadInput(InputUtils.StringConverter, Program.consoleInputValidator.ValidateFirstName);

            Console.Write("Last name: ");
            var lastName = ConsoleParameterReader.ReadInput(InputUtils.StringConverter, Program.consoleInputValidator.ValidateLastName);

            Console.Write("Date of birth: ");
            var birthDate = ConsoleParameterReader.ReadInput(InputUtils.DateTimeConverter, Program.consoleInputValidator.ValidateBirthDay);

            Console.Write("Stature: ");
            var stature = ConsoleParameterReader.ReadInput(InputUtils.ShortConverter, Program.consoleInputValidator.ValidateStature);

            Console.Write("Weight: ");
            var weight = ConsoleParameterReader.ReadInput(InputUtils.DecimalConverter, Program.consoleInputValidator.ValidateWeight);

            Console.Write("Gender: ");
            var gender = ConsoleParameterReader.ReadInput(InputUtils.CharConverter, Program.consoleInputValidator.ValidateGender);

            var parameterObject = new FileCabinetRecordParameterObject(id, firstName, lastName, birthDate, stature, gender, weight);
            Program.fileCabinetService.EditRecord(parameterObject);

            Console.WriteLine($"Record #{id} is updated.");
        }

        private static void Find(string parameters)
        {
            var values = parameters.Split(' ', 2);

            if (values.Length < 2)
            {
                Console.WriteLine("Parameter name or parameter value is missing!");
                return;
            }

            var paramName = values[0];
            var paramValue = values[1].Trim();

            if (paramValue.Length < 3)
            {
                Console.WriteLine("Parameter value is missing!");
                return;
            }

            paramValue = values[1].Substring(1, values[1].Length - 2);

            ReadOnlyCollection<FileCabinetRecord> records;
            if (paramName.Equals("firstname", StringComparison.InvariantCultureIgnoreCase))
            {
                records = Program.fileCabinetService.FindByFirstName(paramValue);
            }
            else if (paramName.Equals("lastname", StringComparison.InvariantCultureIgnoreCase))
            {
                records = Program.fileCabinetService.FindByLastName(paramValue);
            }
            else if (paramName.Equals("dateofbirth", StringComparison.InvariantCultureIgnoreCase))
            {
                DateTime dateOfBirth;
                if (!DateTime.TryParse(paramValue, out dateOfBirth))
                {
                    Console.WriteLine("Invalid format of date!");
                    return;
                }

                records = Program.fileCabinetService.FindByDateOfBirth(dateOfBirth);
            }
            else
            {
                Console.WriteLine("Search criteria by specified parameter is not defined!");
                return;
            }

            foreach (var record in records)
            {
                Console.WriteLine($"#{record}");
            }
        }

        private static void Export(string parameters)
        {
            if (Program.fileCabinetService is FileCabinetFilesystemService)
            {
                Console.WriteLine("This command is not allowed with --storage=file");
                return;
            }

            var parametersValues = parameters.Split(' ');

            if (parametersValues.Length < 2)
            {
                Console.WriteLine("Missing export method or file path.");
                return;
            }

            var exportMethod = parametersValues[0];
            if (!exportMethod.Equals("csv", StringComparison.InvariantCultureIgnoreCase) && !exportMethod.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine($"Unsupported export method: {exportMethod}");
                return;
            }

            var snapshotFilePath = parametersValues[1];
            if (File.Exists(snapshotFilePath))
            {
                Console.Write($"File is exist - rewrite {snapshotFilePath}? [Y/n] ");
                var userAnswer = Console.ReadKey().KeyChar;
                Console.WriteLine();

                userAnswer = char.ToUpperInvariant(userAnswer);
                if (userAnswer != 'Y')
                {
                    return;
                }
            }

            try
            {
                using (var writer = new StreamWriter(snapshotFilePath))
                {
                    var snapshot = ((FileCabinetMemoryService)Program.fileCabinetService).MakeSnapshot();

                    if (exportMethod.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
                    {
                        snapshot.SaveToCsv(writer);
                    }
                    else if (exportMethod.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
                    {
                        snapshot.SaveToXml(writer);
                    }

                    writer.Close();
                }
            }
            catch (IOException exception)
            {
                Console.WriteLine($"Export failed: {exception.Message}");
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                Console.WriteLine($"Access error: {unauthorizedAccessException.Message}");
            }
        }

        private static void Import(string parameters)
        {
            var parametersValues = parameters.Split(' ');

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

                    importedRecordsCount = Program.fileCabinetService.Restore(snapshot, (record, message) => Console.WriteLine($"Import of record with id : {record.Id} failed with error: {message}"));
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

        private static void Remove(string parameters)
        {
            int recordId;
            if (!int.TryParse(parameters, out recordId))
            {
                Console.WriteLine($"Invalid id '{parameters}'.");
                return;
            }

            if (recordId < 1)
            {
                Console.WriteLine($"Id can't be lower than 1.");
                return;
            }

            if (!Program.fileCabinetService.RecordExists(recordId))
            {
                Console.WriteLine($"Record #{recordId} doesn't exists.");
                return;
            }

            Program.fileCabinetService.RemoveRecord(recordId);

            Console.WriteLine($"Record #{recordId} is removed.");
        }

        private static void Purge(string parameters)
        {
            if (Program.fileCabinetService is not FileCabinetFilesystemService)
            {
                Console.WriteLine("This command is only allowed for the filesystem service.");
                return;
            }

            var fileSystemService = (FileCabinetFilesystemService)Program.fileCabinetService;

            var recordsAmount = fileSystemService.GetStat().total;

            fileSystemService.Purge();

            var purgedRecords = recordsAmount - Program.fileCabinetService.GetStat().total;

            Console.WriteLine($"Data file processing is completed: {purgedRecords} of {recordsAmount} records were purged.");
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }
    }
}