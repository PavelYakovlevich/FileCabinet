using System;
using System.Text;
using System.Text.RegularExpressions;

using FileCabinetApp.Configuration;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Console input validator.
    /// </summary>
    public class ConsoleInputValidator : IConsoleInputValidator
    {
        private ValidationConfig validationConfig;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConsoleInputValidator"/> class.
        /// </summary>
        /// <param name="validationConfig"><see cref="ValidationConfig"/> object, which have all validation constraints.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="validationConfig"/> is null.</exception>
        public ConsoleInputValidator(ValidationConfig validationConfig)
        {
            Guard.ArgumentIsNotNull(validationConfig, nameof(validationConfig));

            this.validationConfig = validationConfig;
        }

        /// <inheritdoc cref="IConsoleInputValidator.ValidateBirthDay(DateTime)"/>
        public Tuple<bool, string> ValidateBirthDay(DateTime dateOfBirth)
        {
            const string dateTimeFormat = "MM-dd-yyyy";

            var fromDate = this.validationConfig.DateOfBirth.From;
            var toDate = this.validationConfig.DateOfBirth.To;

            if (dateOfBirth.CompareTo(fromDate) < 0 || dateOfBirth.CompareTo(toDate) > 0)
            {
                return new Tuple<bool, string>(false, $"Date of birth must be in range of {fromDate.ToString(dateTimeFormat)}...{toDate.ToString(dateTimeFormat)}");
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

            var minLength = this.validationConfig.FirstName.MinLength;
            var maxLength = this.validationConfig.FirstName.MaxLength;

            if (!(firstName.Length >= minLength && firstName.Length <= maxLength))
            {
                return new Tuple<bool, string>(false, $"Firstname length must be in range of {minLength}..{maxLength}");
            }

            var regex = new Regex(@"^[A-Za-z]+$");
            if (!regex.IsMatch(firstName))
            {
                return new Tuple<bool, string>(false, $"Firstname must be consist of letters only");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <inheritdoc cref="IConsoleInputValidator.ValidateGender(char)"/>
        public Tuple<bool, string> ValidateGender(char gender)
        {
            var genders = this.validationConfig.Gender;
            var validGenders = new StringBuilder();
            foreach (var validGender in genders)
            {
                validGenders.Append($"{validGender},");

                if (validGender == gender)
                {
                    return new Tuple<bool, string>(true, string.Empty);
                }
            }

            return new Tuple<bool, string>(false, $"Gender must be equal to {validGenders.ToString().Substring(0, validGenders.Length - 1)}");
        }

        /// <inheritdoc cref="IConsoleInputValidator.ValidateLastName(string)"/>
        public Tuple<bool, string> ValidateLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName) || string.IsNullOrWhiteSpace(lastName))
            {
                return new Tuple<bool, string>(false, $"Lastname can't be empty or whitespace");
            }

            var minLength = this.validationConfig.LastName.MinLength;
            var maxLength = this.validationConfig.LastName.MaxLength;

            if (!(lastName.Length >= minLength && lastName.Length <= maxLength))
            {
                return new Tuple<bool, string>(false, $"Lastname length must be in range of {minLength}..{maxLength}");
            }

            var regex = new Regex(@"^[A-Za-z]+$");
            if (!regex.IsMatch(lastName))
            {
                return new Tuple<bool, string>(false, $"Lastname must be consist of letters only");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <inheritdoc cref="IConsoleInputValidator.ValidateStature(short)"/>
        public Tuple<bool, string> ValidateStature(short stature)
        {
            var minValue = this.validationConfig.Stature.MinValue;
            var maxValue = this.validationConfig.Stature.MaxValue;

            if (stature < minValue || stature >= maxValue)
            {
                return new Tuple<bool, string>(false, $"Stature must be greater than {minValue} and less than {maxValue}");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <inheritdoc cref="IConsoleInputValidator.ValidateWeight(decimal)"/>
        public Tuple<bool, string> ValidateWeight(decimal weight)
        {
            var minValue = this.validationConfig.Weight.MinValue;
            var maxValue = this.validationConfig.Weight.MaxValue;

            if (weight < minValue || weight >= maxValue)
            {
                return new Tuple<bool, string>(false, $"Weight must be greater than {minValue} and less than {maxValue}");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }
    }
}
