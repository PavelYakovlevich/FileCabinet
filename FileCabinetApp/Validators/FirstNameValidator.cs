using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     First name parametrized validator.
    /// </summary>
    public class FirstNameValidator : FileRecordValidatorBase
    {
        private readonly int minLength;
        private readonly int maxLength;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FirstNameValidator"/> class.
        /// </summary>
        /// <param name="minLength">Minimal length of the first name.</param>
        /// <param name="maxLength">Maximum length of the first name.</param>
        public FirstNameValidator(int minLength, int maxLength)
        {
            Guard.ArgumentGreaterThan(minLength, -1, $"{nameof(minLength)} must be positive.");
            Guard.ArgumentGreaterThan(maxLength, -1, $"{nameof(maxLength)} must be positive.");

            Guard.ArgumentLessThan(minLength, maxLength, $"{nameof(minLength)} must be less or equal to {nameof(maxLength)}");

            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        /// <summary>
        ///     Validates first name.
        /// </summary>
        /// <param name="record">Object to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown when firstname is null or empty or whitespace.</exception>
        /// <exception cref="ArgumentException">Thrown when lastname's length is less than minimal length or greater than maximal length or value contains non letters chars.</exception>
        public override void Validate(FileCabinetRecord record)
        {
            Guard.ArgumentIsNotNull(record, nameof(record));

            Guard.ArgumentIsNotNull(record.FirstName, nameof(record.FirstName));
            Guard.ArgumentIsNotEmptyOrWhiteSpace(record.FirstName, nameof(record.FirstName));
            Guard.ArgumentMatchRegex(record.FirstName, nameof(record.FirstName), $@"^[A-Za-z]{{{this.minLength},{this.maxLength}}}$");
        }
    }
}