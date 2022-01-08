using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Printers;
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
        private static readonly string StorageAttributeName = "storage";

        private static readonly ProgramInputArgument[] ProgramDefinedArguments = new[]
        {
            new ProgramInputArgument(ValidationRulesAttributeName, "v", new[] { "default", "custom" }),
            new ProgramInputArgument(StorageAttributeName, "s", new[] { "memory", "file" }),
        };

        private static readonly string DBFilePath = @"cabinet-records.db";
        private static FileStream? databaseFileStream;

        private static IConsoleInputValidator consoleInputValidator = new DefaultConsoleInputValidator();
        private static IFileCabinetService fileCabinetService = new FileCabinetMemoryService(new DefaultFileRecordValidator());

        private static bool isRunning = true;

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

            var commandHandler = CreateCommandHandlers();

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

                const int parametersIndex = 1;
                var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;

                commandHandler.Handle(new AppCommandRequest(command, parameters));
            }
            while (isRunning);

            DisposeResources();
        }

        private static void DisposeResources()
        {
            databaseFileStream?.Close();
        }

        private static void HandleArguments(Dictionary<string, string> inputArguments)
        {
            IRecordValidator recordValidator;
            if (inputArguments.ContainsKey(ValidationRulesAttributeName))
            {
                var argumentValue = inputArguments[ValidationRulesAttributeName].ToLower();

                if (argumentValue.Equals("custom"))
                {
                    recordValidator = new CustomFileRecordValidator();
                    consoleInputValidator = new CustomConsoleInputValidator();
                }
                else
                {
                    recordValidator = new DefaultFileRecordValidator();
                }

                Console.WriteLine($"Using {argumentValue} validation rules.");
            }
            else
            {
                recordValidator = new DefaultFileRecordValidator();
                Console.WriteLine($"Using default validation rules.");
            }

            if (inputArguments.ContainsKey(StorageAttributeName))
            {
                var argumentValue = inputArguments[StorageAttributeName].ToLower();

                if (argumentValue.Equals("file"))
                {
                    databaseFileStream = new FileStream(DBFilePath, FileMode.OpenOrCreate);
                    fileCabinetService = new FileCabinetFilesystemService(recordValidator, databaseFileStream);
                }
                else
                {
                    fileCabinetService = new FileCabinetMemoryService(recordValidator);
                }
            }
            else
            {
                fileCabinetService = new FileCabinetMemoryService(recordValidator);
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

        private static ICommandHandler CreateCommandHandlers()
        {
            var recordsPrinter = new DefaultRecordPrinter();

            var helpHandler = new HelpCommandHandler();
            var createHandler = new CreateCommandHandler(fileCabinetService, consoleInputValidator);
            var editHandler = new EditCommandHandler(fileCabinetService, consoleInputValidator);
            var exitHandler = new ExitCommandHandler((value) => isRunning = value);
            var exportHandler = new ExportCommandHandler(fileCabinetService);
            var findHandler = new FindCommandHandler(fileCabinetService, recordsPrinter);
            var importHandler = new ImportCommandHandler(fileCabinetService);
            var listHandler = new ListCommandHandler(fileCabinetService, recordsPrinter);
            var purgeHandler = new PurgeCommandHandler(fileCabinetService);
            var removeHandler = new RemoveCommandHandler(fileCabinetService);
            var statHandler = new StatCommandHandler(fileCabinetService);
            var missedHandler = new MissedCommandHandler();

            helpHandler.SetNext(createHandler);
            createHandler.SetNext(editHandler);
            editHandler.SetNext(removeHandler);
            removeHandler.SetNext(findHandler);
            findHandler.SetNext(statHandler);
            statHandler.SetNext(purgeHandler);
            purgeHandler.SetNext(listHandler);
            listHandler.SetNext(importHandler);
            importHandler.SetNext(exportHandler);
            exportHandler.SetNext(exitHandler);
            exitHandler.SetNext(missedHandler);

            return helpHandler;
        }
    }
}