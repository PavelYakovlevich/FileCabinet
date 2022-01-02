using System;
using System.Text;

namespace FileCabinetGenerator
{
    public static class Program
    {
        private static readonly string OutputFormatArgumentName = "output-type";
        private static readonly string OutputFilePathArgumentName = "output";
        private static readonly string RecordsAmountArgumentName = "records-amount";
        private static readonly string StartIdArgumentName = "start-id";

        private static readonly IInputParameter[] ProgramInputs = new IInputParameter[]
        {
            new RangeInputArgument(OutputFormatArgumentName, "t", true, new[] { "csv", "xml" }),
            new ValueInputParameter(OutputFilePathArgumentName, "o", true, ArgumentType.FilePath),
            new ValueInputParameter(RecordsAmountArgumentName, "a", true, ArgumentType.PositiveInteger),
            new ValueInputParameter(StartIdArgumentName, "i", true, ArgumentType.PositiveInteger),
        };

        private static string outputFormat = string.Empty;
        private static string outputFilePath = string.Empty;
        private static int recordAmount = 0;
        private static int startId = 0;

        public static void Main(string[] args)
        {
            var parametersReader = new ProgramInputArgumentReader(ProgramInputs);

            var readArgumentsResult = parametersReader.ReadInputArguments(args);
            var inputArguments = readArgumentsResult.Item1;

            if (inputArguments is null)
            {
                Console.WriteLine(readArgumentsResult.Item2);
                PrintReadArgumentsErrorMessage();
                return;
            }

            recordAmount = int.Parse(inputArguments[RecordsAmountArgumentName]);
            outputFilePath = inputArguments[OutputFilePathArgumentName];
            startId = int.Parse(inputArguments[StartIdArgumentName]);
            outputFormat = inputArguments[OutputFormatArgumentName];

            Console.WriteLine($"{recordAmount} records were written to {outputFilePath}");
        }

        private static void PrintReadArgumentsErrorMessage()
        {
            StringBuilder errorMessage = new StringBuilder("Defined arguments:\n");

            foreach (var definedArgument in ProgramInputs)
            {
                string argumentValuesString = string.Empty;

                if (definedArgument is ValueInputParameter)
                {
                    argumentValuesString = (definedArgument as ValueInputParameter) !.ValueType.ToString();
                }
                else if (definedArgument is RangeInputArgument)
                {
                    argumentValuesString = (definedArgument as RangeInputArgument) !.GetValidValuesString();
                }

                errorMessage.AppendLine($"\t--{definedArgument.Name}(-{definedArgument.Abbreviation}) : {argumentValuesString}");
            }

            Console.WriteLine(errorMessage);
        }
    }
}
