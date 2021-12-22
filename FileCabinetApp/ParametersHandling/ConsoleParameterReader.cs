using System;

namespace FileCabinetApp
{
    /// <summary>
    ///     Class for the reading of input parameter's values.
    /// </summary>
    public static class ConsoleParameterReader
    {
        /// <summary>
        ///     Gets or sets the value of failed input attempt message.
        /// </summary>
        /// <value>This message is printed after a failed value input attempt.</value>
        public static string FailedInputAttemptMessage { get; set; } = "Value is not valid. Please, try again.";

        /// <summary>
        ///     Reads values from the console with values validation.
        /// </summary>
        /// <typeparam name="T">Type of the returned value.</typeparam>
        /// <param name="converter">
        ///     Contains function, which converts input string to value with type <typeparamref name="T"/>.
        /// </param>
        /// <param name="validationChain">
        ///     Contains chain with the value validation functions.
        /// </param>
        /// <param name="hintMessage">
        ///     Contains hint message with the additional information.
        ///     If specified, then printed after a failed input attempt.
        /// </param>
        /// <returns>Read from console value of type <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when converter or validationChain is null.</exception>
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
