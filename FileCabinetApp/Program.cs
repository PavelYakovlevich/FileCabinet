using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using FileCabinetApp.Services;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    ///     Main class of the program.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Pavel Yakovlevich";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static readonly string ValidationRulesAttributeName = "validation-rules";

        private static readonly ProgramInputArgument[] ProgramDefinedArguments = new[]
        {
            new ProgramInputArgument(ValidationRulesAttributeName, "v", new[] { "default", "custom" }),
        };

        private static IFileCabinetService fileCabinetService = new FileCabinetService(new DefaultFileRecordValidator());
        private static IConsoleInputValidator consoleInputValidator = new DefaultConsoleInputValidator();

        private static bool isRunning = true;

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
        };

        /// <summary>
        ///     Entry point of the program.
        /// </summary>
        /// <param name="args">Arguments, which were passed to the program through the console.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");

            var readArgumentsResult = ReadInputArguments(args);

            if (readArgumentsResult.Item1 is null)
            {
                Console.WriteLine(readArgumentsResult.Item2);
                PrintReadArgumentsErrorMessage();
                return;
            }

            HandleArguments(readArgumentsResult.Item1);

            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine() !.Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void HandleArguments(Dictionary<string, string> inputArguments)
        {
            if (inputArguments.ContainsKey(ValidationRulesAttributeName))
            {
                var argumentValue = inputArguments[ValidationRulesAttributeName].ToLower();

                if (argumentValue.Equals("custom"))
                {
                    var recordValidator = new CustomFileRecordValidator();
                    consoleInputValidator = new CustomConsoleInputValidator();

                    fileCabinetService = new FileCabinetService(recordValidator);
                }

                Console.WriteLine($"Using {argumentValue} validation rules.");
            }
            else
            {
                Console.WriteLine($"Using default validation rules.");
            }
        }

        private static void PrintReadArgumentsErrorMessage()
        {
            StringBuilder errorMessage = new StringBuilder("Defined arguments:\n");

            foreach (var definedArgument in ProgramDefinedArguments)
            {
                errorMessage.AppendLine($"\t--{definedArgument.Name}(-{definedArgument.Abbreviation}) : {definedArgument.GetValidValuesString()}");
            }

            Console.WriteLine(errorMessage);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
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
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            Console.Write("First name: ");
            var firstName = ConsoleParameterReader.ReadInput(InputUtils.StringConverter, consoleInputValidator.ValidateFirstName);

            Console.Write("Last name: ");
            var lastName = ConsoleParameterReader.ReadInput(InputUtils.StringConverter, consoleInputValidator.ValidateLastName);

            Console.Write("Date of birth: ");
            var birthDate = ConsoleParameterReader.ReadInput(InputUtils.DateTimeConverter, consoleInputValidator.ValidateBirthDay);

            Console.Write("Stature: ");
            var stature = ConsoleParameterReader.ReadInput(InputUtils.ShortConverter, consoleInputValidator.ValidateStature);

            Console.Write("Weight: ");
            var weight = ConsoleParameterReader.ReadInput(InputUtils.DecimalConverter, consoleInputValidator.ValidateWeight);

            Console.Write("Gender: ");
            var gender = ConsoleParameterReader.ReadInput(InputUtils.CharConverter, consoleInputValidator.ValidateGender);

            var parameterObject = new FileCabinetRecordParameterObject(firstName, lastName, birthDate, stature, gender, weight);
            var createdRecIndex = fileCabinetService.CreateRecord(parameterObject);

            Console.WriteLine($"Record #{createdRecIndex} is created.");
        }

        private static void List(string parameters)
        {
            var records = fileCabinetService.GetRecords();

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

            if (!fileCabinetService.RecordExists(id))
            {
                Console.WriteLine($"#{id} record is not found.");
                return;
            }

            Console.Write("First name: ");
            var firstName = ConsoleParameterReader.ReadInput(InputUtils.StringConverter, consoleInputValidator.ValidateFirstName);

            Console.Write("Last name: ");
            var lastName = ConsoleParameterReader.ReadInput(InputUtils.StringConverter, consoleInputValidator.ValidateLastName);

            Console.Write("Date of birth: ");
            var birthDate = ConsoleParameterReader.ReadInput(InputUtils.DateTimeConverter, consoleInputValidator.ValidateBirthDay);

            Console.Write("Stature: ");
            var stature = ConsoleParameterReader.ReadInput(InputUtils.ShortConverter, consoleInputValidator.ValidateStature);

            Console.Write("Weight: ");
            var weight = ConsoleParameterReader.ReadInput(InputUtils.DecimalConverter, consoleInputValidator.ValidateWeight);

            Console.Write("Gender: ");
            var gender = ConsoleParameterReader.ReadInput(InputUtils.CharConverter, consoleInputValidator.ValidateGender);

            var parameterObject = new FileCabinetRecordParameterObject(id, firstName, lastName, birthDate, stature, gender, weight);
            fileCabinetService.EditRecord(parameterObject);

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
                records = fileCabinetService.FindByFirstName(paramValue);
            }
            else if (paramName.Equals("lastname", StringComparison.InvariantCultureIgnoreCase))
            {
                records = fileCabinetService.FindByLastName(paramValue);
            }
            else if (paramName.Equals("dateofbirth", StringComparison.InvariantCultureIgnoreCase))
            {
                DateTime dateOfBirth;
                if (!DateTime.TryParse(paramValue, out dateOfBirth))
                {
                    Console.WriteLine("Invalid format of date!");
                    return;
                }

                records = fileCabinetService.FindByDateOfBirth(dateOfBirth);
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
                    SaveSnapshot(writer, exportMethod);
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

        private static void SaveSnapshot(StreamWriter writer, string exportMethod)
        {
            var snapshot = fileCabinetService.MakeSnapshot();

            if (exportMethod.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
            {
                snapshot.SaveToCsv(writer);
            }
        }

        private static Tuple<Dictionary<string, string>?, string> ReadInputArguments(string[] args)
        {
            var result = new Dictionary<string, string>();

            var definedArgumentIndex = 0;
            var currentArgsElementIndex = 0;
            while (currentArgsElementIndex < args.Length && definedArgumentIndex < ProgramDefinedArguments.Length)
            {
                var readArgumentResult = ReadInputArgument(args, ref currentArgsElementIndex);

                if (!readArgumentResult.Item3.Equals(string.Empty))
                {
                    return new Tuple<Dictionary<string, string>?, string>(null, readArgumentResult.Item3);
                }

                readArgumentResult = PreprocessInputArgument((readArgumentResult.Item1, readArgumentResult.Item2));

                if (!readArgumentResult.Item3.Equals(string.Empty))
                {
                    return new Tuple<Dictionary<string, string>?, string>(null, readArgumentResult.Item3);
                }

                result.Add(readArgumentResult.Item1, readArgumentResult.Item2);
                definedArgumentIndex++;
            }

            return new Tuple<Dictionary<string, string>?, string>(result, string.Empty);
        }

        private static (string, string, string) PreprocessInputArgument((string, string) argumentData)
        {
            var argumentName = argumentData.Item1;
            foreach (var definedArgument in ProgramDefinedArguments)
            {
                if (argumentName.Equals(definedArgument.Name, StringComparison.InvariantCultureIgnoreCase) ||
                    argumentName.Equals(definedArgument.Abbreviation, StringComparison.InvariantCultureIgnoreCase))
                {
                    var argumentValue = argumentData.Item2;

                    if (definedArgument.ValidateValue(argumentValue))
                    {
                        return (definedArgument.Name, argumentValue, string.Empty);
                    }

                    return (argumentData.Item1, argumentData.Item2, $"Invalid value '{argumentValue}' for the '{argumentName}' argument.");
                }
            }

            return (argumentData.Item1, argumentData.Item2, $"Argument '{argumentName}' is not defined.");
        }

        private static (string, string, string) ReadInputArgument(string[] args, ref int currentArgsIndex)
        {
            (string, string, string) result = (string.Empty, string.Empty, string.Empty);

            var currentArgument = args[currentArgsIndex];
            var trimmedArgument = currentArgument.TrimStart('-');

            if (string.IsNullOrEmpty(trimmedArgument))
            {
                result.Item3 = $"Missing argument name for the input argument[{currentArgsIndex}]";
                return result;
            }

            if (currentArgument.StartsWith("--"))
            {
                var splittedData = trimmedArgument.Split('=');

                if (splittedData.Length < 2)
                {
                    result.Item3 = $"Missing value for the '{trimmedArgument}' argument.";
                    return result;
                }

                result.Item1 = splittedData[0];
                result.Item2 = splittedData[1];

                currentArgsIndex++;
            }
            else if (currentArgument.StartsWith('-'))
            {
                if (currentArgsIndex + 1 == args.Length)
                {
                    result.Item3 = $"Missing value for the '{trimmedArgument}' argument.";
                    return result;
                }

                result.Item1 = trimmedArgument;
                result.Item2 = args[currentArgsIndex + 1];

                currentArgsIndex += 2;
            }
            else
            {
                result.Item3 = $"Invalid parameter format: '{trimmedArgument}'";
            }

            return result;
        }
    }
}