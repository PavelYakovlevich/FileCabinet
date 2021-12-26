using System;
using System.Text.RegularExpressions;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Custom console input validator.
    /// </summary>
    public class CustomConsoleInputValidator : IConsoleInputValidator
    {
        /// <inheritdoc cref="IConsoleInputValidator.ValidateBirthDay(DateTime)"/>
        public Tuple<bool, string> ValidateBirthDay(DateTime dateOfBirth)
        {
            var minimalDateOfBirth = new DateTime(1950, 1, 1);
            var dateTimeFormat = "MM-dd-yyyy";

            if (dateOfBirth.CompareTo(minimalDateOfBirth) < 0 || dateOfBirth.CompareTo(DateTime.Now) > 0)
            {
                return new Tuple<bool, string>(false, $"Date of birth must be in range of {minimalDateOfBirth.ToString(dateTimeFormat)}...{DateTime.Now.ToString(dateTimeFormat)}");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <inheritdoc cref="IConsoleInputValidator.ValidateFirstName(string)"/>
        public Tuple<bool, string> ValidateFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrWhiteSpace(firstName))
            {
                return new Tuple<bool, string>(false, $"Firstname can't be empty or whitespace");
            }

            if (!(firstName.Length >= 2 && firstName.Length <= 60))
            {
                return new Tuple<bool, string>(false, $"Firstname length must be in range of 2..60");
            }

            var regex = new Regex(@"^[A-Za-z]{2,60}$");
            if (!regex.IsMatch(firstName))
            {
                return new Tuple<bool, string>(false, $"Firstname must be consist of letters only");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <inheritdoc cref="IConsoleInputValidator.ValidateGender(char)"/>
        public Tuple<bool, string> ValidateGender(char gender)
        {
            if (gender != 'M' && gender != 'F')
            {
                return new Tuple<bool, string>(false, $"Gender must be equal to M or F");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <inheritdoc cref="IConsoleInputValidator.ValidateLastName(string)"/>
        public Tuple<bool, string> ValidateLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName) || string.IsNullOrWhiteSpace(lastName))
            {
                return new Tuple<bool, string>(false, $"Lastname can't be empty or whitespace");
            }

            if (!(lastName.Length >= 2 && lastName.Length <= 60))
            {
                return new Tuple<bool, string>(false, $"Lastname length must be in range of 2..60");
            }

            var regex = new Regex(@"^[A-Za-z]{2,60}$");
            if (!regex.IsMatch(lastName))
            {
                return new Tuple<bool, string>(false, $"Lastname must be consist of letters only");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <inheritdoc cref="IConsoleInputValidator.ValidateStature(short)"/>
        public Tuple<bool, string> ValidateStature(short stature)
        {
            if (stature < 1 || stature >= 300)
            {
                return new Tuple<bool, string>(false, $"Stature must be greater than 0 and less than 300");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <inheritdoc cref="IConsoleInputValidator.ValidateWeight(decimal)"/>
        public Tuple<bool, string> ValidateWeight(decimal weight)
        {
            if (weight < 1 || weight >= 1000m)
            {
                return new Tuple<bool, string>(false, $"Weight must be greater than 0 and less than 1000");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }
    }
}
