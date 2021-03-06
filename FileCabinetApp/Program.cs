using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Configuration;
using FileCabinetApp.Printing;
using FileCabinetApp.Services;
using FileCabinetApp.Utils;
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

        private static readonly string ValidationRulesFileName = @"validation-rules.json";
        private static readonly string ValidationRulesFilePath = @$"{Directory.GetCurrentDirectory()}\Properties\{ValidationRulesFileName}";

        private static readonly string LogFileName = @"log.txt";
        private static readonly string LogFilePath = @$"{Directory.GetCurrentDirectory()}\{LogFileName}";

        private static readonly string ValidationRulesParameterName = "validation-rules";
        private static readonly string StorageParameterName = "storage";
        private static readonly string ServiceMeterParameterName = "use-stopwatch";
        private static readonly string LoggerParameterName = "use-logger";

        private static readonly IInputParameter[] ProgramDefinedArguments = new IInputParameter[]
        {
            new RangeInputParameter(ValidationRulesParameterName, "v", false, new[] { "default", "custom" }),
            new RangeInputParameter(StorageParameterName, "s", false, new[] { "memory", "file" }),
            new FlagInputParameter(ServiceMeterParameterName, false),
            new FlagInputParameter(LoggerParameterName, false),
        };

        private static readonly string DBFilePath = @"cabinet-records.db";
        private static FileStream? databaseFileStream;
        private static FileStream? loggerFileStream;

        private static IInputValidator? consoleInputValidator;
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
            loggerFileStream?.Close();
        }

        private static void HandleArguments(Dictionary<string, string> inputArguments)
        {
            IRecordValidator recordValidator;
            if (inputArguments.ContainsKey(ValidationRulesParameterName))
            {
                var argumentValue = inputArguments[ValidationRulesParameterName].ToLower();

                if (argumentValue.Equals("custom"))
                {
                    recordValidator = new ValidatorBuilder().CreateCustom(ValidationRulesFilePath);
                    consoleInputValidator = new DefaultInputValidator(ReadValidationRules(argumentValue));
                }
                else
                {
                    recordValidator = new ValidatorBuilder().CreateDefault(ValidationRulesFilePath);
                    consoleInputValidator = new DefaultInputValidator(ReadValidationRules(argumentValue));
                }

                Console.WriteLine($"Using {argumentValue} validation rules.");
            }
            else
            {
                recordValidator = new ValidatorBuilder().CreateDefault(ValidationRulesFilePath);
                consoleInputValidator = new DefaultInputValidator(ReadValidationRules("default"));

                Console.WriteLine($"Using default validation rules.");
            }

            if (inputArguments.ContainsKey(StorageParameterName))
            {
                var argumentValue = inputArguments[StorageParameterName].ToLower();

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

            if (inputArguments.ContainsKey(ServiceMeterParameterName))
            {
                fileCabinetService = new ServiceMeter(fileCabinetService);
            }

            if (inputArguments.ContainsKey(LoggerParameterName))
            {
                loggerFileStream = new FileStream(LogFilePath, FileMode.OpenOrCreate);
                fileCabinetService = new ServiceLogger(loggerFileStream, fileCabinetService);
            }

            RecordsUtils.Initialize(consoleInputValidator!);
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
                    validValuesStr = (definedArgument as ValueInputParameter) !.ValueType.ToString();
                }
                else if (definedArgument is FlagInputParameter)
                {
                    validValuesStr = "none";
                }

                errorMessage.AppendLine($"\t{definedArgument.Name}({definedArgument.Abbreviation}) : {validValuesStr}");
            }

            Console.WriteLine(errorMessage);
        }

        private static ICommandHandler CreateCommandHandlers()
        {
            var helpHandler = new HelpCommandHandler();
            var createHandler = new CreateCommandHandler(fileCabinetService, consoleInputValidator!);
            var exitHandler = new ExitCommandHandler((value) => isRunning = value);
            var exportHandler = new ExportCommandHandler(fileCabinetService);
            var importHandler = new ImportCommandHandler(fileCabinetService);
            var purgeHandler = new PurgeCommandHandler(fileCabinetService);
            var statHandler = new StatCommandHandler(fileCabinetService);
            var insertHandler = new InsertCommandHandler(fileCabinetService);
            var deleteHandler = new DeleteCommandHandler(fileCabinetService);
            var updateHandler = new UpdateCommandHandler(fileCabinetService);
            var selectHandler = new SelectCommandHandler(fileCabinetService, new TablePrinter<FileCabinetRecord>());
            var missedHandler = new MissedCommandHandler();

            helpHandler.SetNext(createHandler)
            .SetNext(statHandler)
            .SetNext(purgeHandler)
            .SetNext(importHandler)
            .SetNext(exportHandler)
            .SetNext(insertHandler)
            .SetNext(deleteHandler)
            .SetNext(updateHandler)
            .SetNext(selectHandler)
            .SetNext(exitHandler)
            .SetNext(missedHandler);

            return helpHandler;
        }
    }
}