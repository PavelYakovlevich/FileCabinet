using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Configuration;
using FileCabinetApp.Printers;
using FileCabinetApp.Services;
using FileCabinetApp.Validators;
using Microsoft.Extensions.Configuration;
using ProgramInputHandling;

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

        private static readonly string ValidationRulesFileName = @"validation-rules.json";
        private static readonly string ValidationRulesFilePath = @$"{Directory.GetCurrentDirectory()}\{ValidationRulesFileName}";

        private static readonly string ValidationRulesAttributeName = "validation-rules";
        private static readonly string StorageAttributeName = "storage";

        private static readonly IInputParameter[] ProgramDefinedArguments = new IInputParameter[]
        {
            new RangeInputParameter(ValidationRulesAttributeName, "v", false, new[] { "default", "custom" }),
            new RangeInputParameter(StorageAttributeName, "s", false, new[] { "memory", "file" }),
        };

        private static readonly string DBFilePath = @"cabinet-records.db";
        private static FileStream? databaseFileStream;

        private static IConsoleInputValidator? consoleInputValidator;
        private static IFileCabinetService fileCabinetService = new FileCabinetMemoryService(new ValidatorBuilder().CreateDefault(ValidationRulesFilePath));

        private static bool isRunning = true;

        /// <summary>
        ///     Entry point of the program.
        /// </summary>
        /// <param name="args">Arguments, which were passed to the program through the console.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");

            var inputParameterReader = new ProgramInputParameterReader(ProgramDefinedArguments);

            var readArgumentsResult = inputParameterReader.ReadInputArguments(args);

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
                    recordValidator = new ValidatorBuilder().CreateCustom(ValidationRulesFilePath);
                    consoleInputValidator = new ConsoleInputValidator(ReadValidationRules(argumentValue));
                }
                else
                {
                    recordValidator = new ValidatorBuilder().CreateDefault(ValidationRulesFilePath);
                    consoleInputValidator = new ConsoleInputValidator(ReadValidationRules(argumentValue));
                }

                Console.WriteLine($"Using {argumentValue} validation rules.");
            }
            else
            {
                recordValidator = new ValidatorBuilder().CreateDefault(ValidationRulesFilePath);
                consoleInputValidator = new ConsoleInputValidator(ReadValidationRules("default"));

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

        private static ValidationConfig ReadValidationRules(string validationRuleName)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(ValidationRulesFilePath)
                .Build();

            var validationConfig = config.GetSection(validationRuleName).Get<ValidationConfig>();

            return validationConfig;
        }

        private static void PrintReadArgumentsErrorMessage()
        {
            StringBuilder errorMessage = new StringBuilder("Defined arguments:\n");

            foreach (var definedArgument in ProgramDefinedArguments)
            {
                var validValuesStr = string.Empty;
                if (definedArgument is RangeInputParameter)
                {
                    validValuesStr = ((RangeInputParameter)definedArgument).GetValidValuesString();
                }
                else if (definedArgument is ValueInputParameter)
                {
                    validValuesStr = (definedArgument as ValueInputParameter)!.ValueType.ToString();
                }

                errorMessage.AppendLine($"\t--{definedArgument.Name}(-{definedArgument.Abbreviation}) : {validValuesStr}");
            }

            Console.WriteLine(errorMessage);
        }

        private static ICommandHandler CreateCommandHandlers()
        {
            var recordsPrinter = new DefaultRecordPrinter();

            var helpHandler = new HelpCommandHandler();
            var createHandler = new CreateCommandHandler(fileCabinetService, consoleInputValidator!);
            var editHandler = new EditCommandHandler(fileCabinetService, consoleInputValidator!);
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