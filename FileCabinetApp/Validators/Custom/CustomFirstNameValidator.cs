using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Custom first name validator.
    /// </summary>
    public class CustomFirstNameValidator : FileRecordValidatorBase
    {
        /// <summary>
        ///     Validates first name.
        /// </summary>
        /// <param name="record">Object to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown when firstname is null or empty or whitespace.</exception>
        /// <exception cref="ArgumentException">Thrown when firstname's length is less than 2 or greater than 60 or value contains non letters chars.</exception>
        public override void Validate(FileCabinetRecord record)
        {
            Guard.ArgumentIsNotNull(record.FirstName, nameof(record.FirstName));
            Guard.ArgumentIsNotEmptyOrWhiteSpace(record.FirstName, nameof(record.FirstName));
            Guard.ArgumentMatchRegex(record.FirstName, nameof(record.FirstName), @"^[A-Za-z]{2,60}$");
        }
    }
}
