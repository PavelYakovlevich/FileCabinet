using System;
using System.Text.RegularExpressions;

namespace FileCabinetApp
{
    /// <summary>
    ///     Class for the checking of the input parameters.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        ///     Checks if <paramref name="argument"/> is not null.
        /// </summary>
        /// <typeparam name="T">Type of the argument's value.</typeparam>
        /// <param name="argument">Value, which must be checked.</param>
        /// <param name="nameOfArgument">Argument's name.</param>
        /// <exception cref="ArgumentNullException">Thrown when argument is null.</exception>
        public static void ArgumentIsNotNull<T>(T argument, string nameOfArgument)
            where T : class
        {
            if (argument is null)
            {
                throw new ArgumentNullException(nameOfArgument, $"{nameOfArgument} is null!");
            }
        }

        /// <summary>
        ///     Checks if <paramref name="argument"/> of type <see cref="string"/> is not empty or whitespace.
        /// </summary>
        /// <param name="argument">Value, which must be checked.</param>
        /// <param name="nameOfArgument">Argument's name.</param>
        /// <exception cref="ArgumentException">if string is null or empty or whitespace.</exception>
        public static void ArgumentIsNotEmptyOrWhiteSpace(string argument, string nameOfArgument)
        {
            if (string.IsNullOrWhiteSpace(argument) || string.IsNullOrEmpty(argument))
            {
                throw new ArgumentException($"{nameOfArgument} is empty or whitespace.", argument);
            }
        }

        /// <summary>
        ///     Checks if <paramref name="argument"/> of type <see cref="string"/> matchs regular expression <paramref name="regexString"/>.
        /// </summary>
        /// <param name="argument">Value, which must be checked.</param>
        /// <param name="nameOfArgument">Argument's name.</param>
        /// <param name="regexString">Regular expression, which must be applied to the <paramref name="argument"/>.</param>
        /// <exception cref="ArgumentException">if <paramref name="regexString"/> is not a valid regex or <paramref name="argument"/> doesn't match the regular expression.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="regexString"/> is null.</exception>
        public static void ArgumentMatchRegex(string argument, string nameOfArgument, string regexString)
        {
            var regex = new Regex(regexString);

            if (!regex.IsMatch(argument))
            {
                throw new ArgumentException($"{nameOfArgument} does not match the \'{regexString}\' regular expression.", argument);
            }
        }

        /// <summary>
        ///     Checks if <paramref name="argument"/> is less than <paramref name="secondValue"/>.
        /// </summary>
        /// <typeparam name="T">Type of the argument's value.</typeparam>
        /// <param name="argument">Value, which must be checked.</param>
        /// <param name="secondValue">Value, which is compared with <paramref name="argument"/>'s value.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="argument"/> is greater than <paramref name="secondValue"/>.</exception>
        public static void ArgumentLessThan<T>(T argument, T secondValue)
           where T : IComparable<T>
        {
            if (argument.CompareTo(secondValue) >= 0)
            {
                throw new ArgumentException($"{nameof(argument)} is greater than ${secondValue}.");
            }
        }

        /// <summary>
        ///     Checks if <paramref name="argument"/> is greater than <paramref name="secondValue"/>.
        /// </summary>
        /// <typeparam name="T">Type of the argument's value.</typeparam>
        /// <param name="argument">Value, which must be checked.</param>
        /// <param name="secondValue">Value, which is compared with <paramref name="argument"/>'s value.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="argument"/> is less than <paramref name="secondValue"/>.</exception>
        public static void ArgumentGreaterThan<T>(T argument, T secondValue)
           where T : IComparable<T>
        {
            ArgumentGreaterThan(argument, secondValue, $"{nameof(argument)} is lower than ${secondValue}.");
        }

        /// <summary>
        ///     Checks if <paramref name="argument"/> is greater than <paramref name="secondValue"/>.
        /// </summary>
        /// <typeparam name="T">Type of the argument's value.</typeparam>
        /// <param name="argument">Value, which must be checked.</param>
        /// <param name="secondValue">Value, which is compared with <paramref name="argument"/>'s value.</param>
        /// <param name="exceptionMessage">Message, which will be passed to the exception constructor.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="argument"/> is less than <paramref name="secondValue"/>.</exception>
        public static void ArgumentGreaterThan<T>(T argument, T secondValue, string exceptionMessage)
           where T : IComparable<T>
        {
            if (argument.CompareTo(secondValue) <= 0)
            {
                throw new ArgumentException(exceptionMessage);
            }
        }

        /// <summary>
        ///     Checks if <paramref name="argument"/> is equal to one of the <paramref name="validValues"/>'s value.
        /// </summary>
        /// <typeparam name="T">Type of the argument's value.</typeparam>
        /// <param name="argument">Value, which must be checked.</param>
        /// <param name="validValues"><see cref="Array"/> of valid values.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="argument"/> is not equal to any <paramref name="validValues"/> value.</exception>
        public static void ArgumentIsInRange<T>(T argument, T[] validValues)
        {
            ArgumentIsInRange(argument, validValues, $"{argument} is not in the specified range.");
        }

        /// <summary>
        ///     Checks if <paramref name="argument"/> is equal to one of the <paramref name="validValues"/>'s value.
        /// </summary>
        /// <typeparam name="T">Type of the argument's value.</typeparam>
        /// <param name="argument">Value, which must be checked.</param>
        /// <param name="validValues"><see cref="Array"/> of valid values.</param>
        /// <param name="exceptionMessage">Message, which will be passed to the exception constructor.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="argument"/> is not equal to any <paramref name="validValues"/> value.</exception>
        public static void ArgumentIsInRange<T>(T argument, T[] validValues, string exceptionMessage)
        {
            if (argument is null)
            {
                throw new ArgumentNullException(nameof(argument), $"{nameof(argument)} can't be null.");
            }

            foreach (var validValue in validValues)
            {
                if (argument.Equals(validValue))
                {
                    return;
                }
            }

            throw new ArgumentOutOfRangeException(exceptionMessage);
        }

        /// <summary>
        ///     Checks if <paramref name="argument"/> satisfies specified <paramref name="condition"/>'.
        /// </summary>
        /// <typeparam name="T">Type of the argument's value.</typeparam>
        /// <param name="argument">Value, which must be checked.</param>
        /// <param name="condition">Function of type <see cref="Predicate{T}"/> which checks <paramref name="argument"/> for satysfing specified condition.</param>
        /// <param name="exceptionMessage">Message, which will be passed to the exception constructor.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="argument"/> doesn't satisfy <paramref name="condition"/>.</exception>
        public static void ArgumentSatisfies<T>(T argument, Predicate<T> condition, string exceptionMessage)
        {
            if (!condition(argument))
            {
                throw new ArgumentException(exceptionMessage);
            }
        }
    }
}
