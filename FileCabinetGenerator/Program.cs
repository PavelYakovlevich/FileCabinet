using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FileCabinetApp;
using ProgramInputHandling;

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
            new RangeInputParameter(OutputFormatArgumentName, "t", true, new[] { "csv", "xml" }),
            new ValueInputParameter(OutputFilePathArgumentName, "o", true, ArgumentType.FilePath),
            new ValueInputParameter(RecordsAmountArgumentName, "a", true, ArgumentType.PositiveInteger),
            new ValueInputParameter(StartIdArgumentName, "i", true, ArgumentType.PositiveInteger),
        };

        private static string outputFormat = string.Empty;
        private static string outputFilePath = string.Empty;
        private static int recordsAmount = 0;
        private static int startId = 0;

        public static void Main(string[] args)
        {
            var parametersReader = new ProgramInputParameterReader(ProgramInputs);

            var readArgumentsResult = parametersReader.ReadInputArguments(args);

            var inputArguments = readArgumentsResult.Item1;
            if (inputArguments is null)
            {
                Console.WriteLine(readArgumentsResult.Item2);
                PrintReadArgumentsErrorMessage();
                return;
            }

            recordsAmount = int.Parse(inputArguments[RecordsAmountArgumentName]);
            outputFilePath = inputArguments[OutputFilePathArgumentName];
            startId = int.Parse(inputArguments[StartIdArgumentName]);
            outputFormat = inputArguments[OutputFormatArgumentName];

            var recordFaker = new FileCabinetRecordFaker(1950);
            int currentRecordId = startId;

            var records = new FileCabinetRecord[recordsAmount];
            for (int i = 0; i < recordsAmount; i++)
            {
                var generatedRecord = recordFaker.Generate(currentRecordId);
                records[i] = generatedRecord;
                currentRecordId++;
            }

            try
            {
                if (outputFormat.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
                {
                    ExportToXml(records);
                }
                else
                {
                    ExportToCsv(records);
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
            catch (InvalidOperationException exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
            }
            catch
            {
                Console.WriteLine("Ops, something went wrong.");
            }

            Console.WriteLine($"{recordsAmount} records were written to {outputFilePath}");
        }

        private static void ExportToXml(IList<FileCabinetRecord> records)
        {
            using (var stream = new StreamWriter(outputFilePath, false))
            {
                XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
                using (var xmlWriter = XmlWriter.Create(stream, settings))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(FileCabinetRecord[]), new XmlRootAttribute("records"));

                    serializer.Serialize(xmlWriter, records);
                }
            }
        }

        private static void ExportToCsv(IList<FileCabinetRecord> records)
        {
            using (var stream = new StreamWriter(outputFilePath, false))
            {
                foreach (var record in records)
                {
                    var csvString = $"{record.Id},{record.FirstName},{record.LastName},{record.DateOfBirth.ToString("MM/dd/yyyy")},{record.Gender},{record.Stature},{record.Weight}";
                    stream.WriteLine(csvString);
                }
            }
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
                else if (definedArgument is RangeInputParameter)
                {
                    argumentValuesString = (definedArgument as RangeInputParameter) !.GetValidValuesString();
                }

                errorMessage.AppendLine($"\t--{definedArgument.Name}(-{definedArgument.Abbreviation}) : {argumentValuesString}");
            }

            Console.WriteLine(errorMessage);
        }
    }
}
