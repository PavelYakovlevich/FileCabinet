using System;

namespace FileCabinetApp
{
    public static class Guard
    {
        public static void ArgumentIsNotNull<T>(T argument, string nameOfArgument)
            where T : class
        {
            if (argument is null)
            {
                throw new ArgumentNullException(nameOfArgument, $"{nameOfArgument} is null!");
            }
        }

        public static void ArgumentIsNotEmptyOrWhiteSpace(string argument, string nameOfArgument)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ArgumentNullException(nameOfArgument, $"{nameOfArgument} is empty or whitespace.");
            }
        }

        public static void ArgumentGreaterThan<T>(T argument, T secondValue)
           where T : IComparable<T>
        {
            ArgumentGreaterThan(argument, secondValue, $"{nameof(argument)} is lower than ${secondValue}.");
        }

        public static void ArgumentGreaterThan<T>(T argument, T secondValue, string exceptionMessage)
           where T : IComparable<T>
        {
            if (argument.CompareTo(secondValue) <= 0)
            {
                throw new ArgumentException(exceptionMessage);
            }
        }

        public static void ArgumentIsInRange<T>(T argument, T[] validValues)
        {
            ArgumentIsInRange(argument, validValues, $"{argument} is not in the specified range.");
        }

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

        public static void ArgumentSatisfies<T>(T argument, Predicate<T> condition, string exceptionMessage)
        {
            if (!condition(argument))
            {
                throw new ArgumentException(exceptionMessage);
            }
        }
    }
}
