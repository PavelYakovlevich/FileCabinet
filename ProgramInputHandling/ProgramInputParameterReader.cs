using System;
using System.Collections.Generic;

namespace ProgramInputHandling
{
    public class ProgramInputParameterReader
    {
        private readonly IInputParameter[] programDefinedArguments;
        private int mandatoryParamsCount;

        public ProgramInputParameterReader(IInputParameter[] programInputs)
        {
            if (programInputs is null)
            {
                throw new ArgumentNullException(nameof(programInputs), $"{nameof(programInputs)} must be not null.");
            }

            if (programInputs.Length == 0)
            {
                throw new ArgumentException($"{nameof(programInputs)} must be not empty.", nameof(programInputs));
            }

            this.programDefinedArguments = programInputs;

            for (int i = 0; i < programInputs.Length; i++)
            {
                if (programInputs[i].IsMandatory)
                {
                    this.mandatoryParamsCount++;
                }
            }
        }

        public Tuple<Dictionary<string, string>?, string> ReadInputArguments(string[] args)
        {
            var result = new Dictionary<string, string>();

            var definedArgumentIndex = 0;
            var currentArgsElementIndex = 0;
            var mandatoryParametersCount = this.mandatoryParamsCount;
            while (currentArgsElementIndex < args.Length && definedArgumentIndex < this.programDefinedArguments.Length)
            {
                var readArgumentResult = this.ReadInputArgument(args, ref currentArgsElementIndex);

                if (!readArgumentResult.Item3.Equals(string.Empty))
                {
                    return new Tuple<Dictionary<string, string>?, string>(null, readArgumentResult.Item3);
                }

                readArgumentResult = this.PreprocessInputArgument((readArgumentResult.Item1, readArgumentResult.Item2));

                if (!readArgumentResult.Item3.Equals(string.Empty))
                {
                    return new Tuple<Dictionary<string, string>?, string>(null, readArgumentResult.Item3);
                }

                if (!result.TryAdd(readArgumentResult.Item1, readArgumentResult.Item2))
                {
                    return new Tuple<Dictionary<string, string>?, string>(null, $"Parameter {readArgumentResult.Item1} has been specified before.");
                }

                if (this.IsArgumentMandatory(readArgumentResult.Item1))
                {
                    mandatoryParametersCount--;
                }

                definedArgumentIndex++;
            }

            if (mandatoryParametersCount != 0)
            {
                return new Tuple<Dictionary<string, string>?, string>(null, $"{mandatoryParametersCount} mandatory parameters are missed.");
            }

            return new Tuple<Dictionary<string, string>?, string>(result, string.Empty);
        }

        private bool IsArgumentMandatory(string parameterName)
        {
            foreach (var argument in this.programDefinedArguments)
            {
                if (argument.Name.Equals(parameterName) && argument.IsMandatory)
                {
                    return true;
                }
            }

            return false;
        }

        private (string, string, string) PreprocessInputArgument((string, string) argumentData)
        {
            var argumentName = argumentData.Item1;
            foreach (var definedArgument in this.programDefinedArguments)
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

        private (string, string, string) ReadInputArgument(string[] args, ref int currentArgsIndex)
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
