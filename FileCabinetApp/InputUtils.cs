using System;

namespace FileCabinetApp
{
    /// <summary>
    ///     Class for the ant input util functions.
    /// </summary>
    internal class InputUtils
    {
        private delegate bool TryParseHandler<T>(string input, out T value);

        /// <summary>
        ///     Converts <paramref name="input"/>'s value into the <see cref="string"/>.
        /// </summary>
        /// <param name="input">String, which must be converted to <see cref="string"/>.</param>
        /// <returns>
        ///     <see cref="Tuple{T1, T2, T3}"/> object, whose values means:
        ///     conversion result, conversion error message and converted value accordingly.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="input"/> is null.</exception>
        internal static Tuple<bool, string, string> StringConverter(string input)
        {
            Guard.ArgumentIsNotNull(input, nameof(input));

            return new Tuple<bool, string, string>(true, string.Empty, input);
        }

        /// <summary>
        ///     Converts <paramref name="input"/>'s value into the <see cref="DateTime"/>.
        /// </summary>
        /// <param name="input">String, which must be converted to <see cref="DateTime"/>.</param>
        /// <returns>
        ///     <see cref="Tuple{T1, T2, T3}"/> object, whose values means:
        ///     conversion result, conversion error message and converted value accordingly.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="input"/> is null.</exception>
        internal static Tuple<bool, string, DateTime> DateTimeConverter(string input)
        {
            Guard.ArgumentIsNotNull(input, nameof(input));

            return Convert<DateTime>(input, DateTime.TryParse);
        }

        /// <summary>
        ///     Converts <paramref name="input"/>'s value into the <see cref="short"/>.
        /// </summary>
        /// <param name="input">String, which must be converted to <see cref="short"/>.</param>
        /// <returns>
        ///     <see cref="Tuple{T1, T2, T3}"/> object, whose values means:
        ///     conversion result, conversion error message and converted value accordingly.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="input"/> is null.</exception>
        internal static Tuple<bool, string, short> ShortConverter(string input)
        {
            Guard.ArgumentIsNotNull(input, nameof(input));

            return Convert<short>(input, short.TryParse);
        }

        /// <summary>
        ///     Converts <paramref name="input"/>'s value into the <see cref="char"/>.
        /// </summary>
        /// <param name="input">String, which must be converted to <see cref="char"/>.</param>
        /// <returns>
        ///     <see cref="Tuple{T1, T2, T3}"/> object, whose values means:
        ///     conversion result, conversion error message and converted value accordingly.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="input"/> is null.</exception>
        internal static Tuple<bool, string, char> CharConverter(string input)
        {
            Guard.ArgumentIsNotNull(input, nameof(input));

            return Convert<char>(input, char.TryParse);
        }

        /// <summary>
        ///     Converts <paramref name="input"/>'s value into the <see cref="decimal"/>.
        /// </summary>
        /// <param name="input">String, which must be converted to <see cref="decimal"/>.</param>
        /// <returns>
        ///     <see cref="Tuple{T1, T2, T3}"/> object, whose values means:
        ///     conversion result, conversion error message and converted value accordingly.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="input"/> is null.</exception>
        internal static Tuple<bool, string, decimal> DecimalConverter(string input)
        {
            Guard.ArgumentIsNotNull(input, nameof(input));

            return Convert<decimal>(input, decimal.TryParse);
        }

        private static Tuple<bool, string, T> Convert<T>(string input, TryParseHandler<T> parser)
        {
            T parsedValue;

            if (!parser(input, out parsedValue))
            {
                var conversionFailedMessage = BuildConversionFailedMessage(input, typeof(T));

                return new Tuple<bool, string, T>(false, conversionFailedMessage, parsedValue);
            }

            return new Tuple<bool, string, T>(true, string.Empty, parsedValue);
        }

        private static string BuildConversionFailedMessage(string value, Type conversionTargetType)
        {
            return $"\"{value}\" can't be converted to type: {conversionTargetType.FullName}.";
        }
    }
}