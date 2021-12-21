using System;

namespace FileCabinetApp
{
    internal class InputUtils
    {
        private delegate bool TryParseHandler<T>(string input, out T value);

        internal static Tuple<bool, string, string> StringConverter(string input)
        {
            Guard.ArgumentIsNotNull(input, nameof(input));

            return new Tuple<bool, string, string>(true, string.Empty, input);
        }

        internal static Tuple<bool, string, DateTime> DateTimeConverter(string input)
        {
            Guard.ArgumentIsNotNull(input, nameof(input));

            return Convert<DateTime>(input, DateTime.TryParse);
        }

        internal static Tuple<bool, string, short> ShortConverter(string input)
        {
            Guard.ArgumentIsNotNull(input, nameof(input));

            return Convert<short>(input, short.TryParse);
        }

        internal static Tuple<bool, string, char> CharConverter(string input)
        {
            Guard.ArgumentIsNotNull(input, nameof(input));

            return Convert<char>(input, char.TryParse);
        }

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