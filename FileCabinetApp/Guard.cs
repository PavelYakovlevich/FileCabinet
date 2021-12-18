using System;

namespace FileCabinetApp
{
    public class Guard
    {
        public Guard IsNotNull<T>(T value, string nameOfValue)
            where T : class
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameOfValue, $"{nameOfValue} is null!");
            }

            return this;
        }

        public Guard IsNotEmptyOrWhiteSpace(string value, string nameOfValue)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameOfValue, $"{nameOfValue} is empty or whitespace.");
            }

            return this;
        }

        public Guard GreaterThan<T>(T value1, T value2)
           where T : IComparable<T>
        {
            return this.GreaterThan(value1, value2, $"{nameof(value1)} is lower than ${value2}");
        }

        public Guard GreaterThan<T>(T value1, T value2, string message)
           where T : IComparable<T>
        {
            if (value1.CompareTo(value2) <= 0)
            {
                throw new ArgumentException(message);
            }

            return this;
        }

        public Guard IsInRange<T>(T value, T[] validValues)
        {
            return this.IsInRange(value, validValues, $"{value} is not in the specified range.");
        }

        public Guard IsInRange<T>(T value, T[] validValues, string message)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value), $"{nameof(value)} can't be null.");
            }

            foreach (var validValue in validValues)
            {
                if (value.Equals(validValues))
                {
                    return this;
                }
            }

            throw new ArgumentOutOfRangeException(message);
        }

        public Guard Requires(Func<bool> condition, string message)
        {
            if (!condition())
            {
                throw new ArgumentException(message);
            }

            return this;
        }
    }
}
