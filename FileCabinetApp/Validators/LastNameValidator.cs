using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Last name parametrized validator.
    /// </summary>
    public class LastNameValidator : FileRecordValidatorBase
    {
        private readonly int minLength;
        private readonly int maxLength;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LastNameValidator"/> class.
        /// </summary>
        /// <param name="minLength">Minimal length of the last name.</param>
        /// <param name="maxLength">Maximum length of the last name.</param>
        public LastNameValidator(int minLength, int maxLength)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        /// <summary>
        ///     Validates last name.
        /// </summary>
        /// <param name="record">Object to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown when lastname is null or empty or whitespace.</exception>
        /// <exception cref="ArgumentException">Thrown when lastname's length is less than minimal length or greater than maximal length or value contains non letters chars.</exception>
        public override void Validate(FileCabinetRecord record)
        {
            Guard.ArgumentIsNotNull(record.LastName, nameof(record.LastName));
            Guard.ArgumentIsNotEmptyOrWhiteSpace(record.LastName, nameof(record.LastName));
            Guard.ArgumentMatchRegex(record.LastName, nameof(record.LastName), $@"^[A-Za-z]{{{this.minLength},{this.maxLength}}}$");
        }
    }
}