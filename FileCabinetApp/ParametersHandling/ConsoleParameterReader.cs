using System;

namespace FileCabinetApp
{
    public static class ConsoleParameterReader
    {
        public static string FailedInputAttemptMessage { get; set; } = "Value is not valid. Please, try again.";

        public static T ReadValue<T>(Func<string, Tuple<bool, string, T>> converter, ParameterValidationChain<T> validationChain, string? hintMessage = null)
        {
            Guard.ArgumentIsNotNull(converter, nameof(converter));
            Guard.ArgumentIsNotNull(validationChain, nameof(validationChain));

            bool hintMessageIsDefined = !(string.IsNullOrEmpty(hintMessage) || string.IsNullOrWhiteSpace(hintMessage));
            do
            {
                var input = Console.ReadLine();
                var conversionResult = converter(input!);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                var value = conversionResult.Item3;

                var valueIsValid = validationChain.Validate(value);
                if (!valueIsValid)
                {
                    Console.WriteLine(FailedInputAttemptMessage);

                    if (hintMessageIsDefined)
                    {
                        Console.WriteLine($"Hint: ${hintMessage}");
                    }

                    continue;
                }

                return value;
            }
            while (true);
        }
    }
}
