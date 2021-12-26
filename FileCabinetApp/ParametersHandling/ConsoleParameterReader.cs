using System;

namespace FileCabinetApp
{
    /// <summary>
    ///     Class for the reading of input parameter's values.
    /// </summary>
    public static class ConsoleParameterReader
    {
        /// <summary>
        ///     Reads input from the console, convert it to the <typeparamref name="T"/> and validates it.
        /// </summary>
        /// <typeparam name="T">Type of value, that must be read from the console.</typeparam>
        /// <param name="converter">Converter function.</param>
        /// <param name="validator">Validation function.</param>
        /// <returns>Value, that was read from the console and converted to the proper type <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="converter"/> or <paramref name="validator"/> is null.</exception>
        public static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            Guard.ArgumentIsNotNull(converter, nameof(converter));
            Guard.ArgumentIsNotNull(validator, nameof(validator));

            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input!);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }
    }
}
