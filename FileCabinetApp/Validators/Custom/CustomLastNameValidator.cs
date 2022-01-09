using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Custom last name validator.
    /// </summary>
    public class CustomLastNameValidator : FileRecordValidatorBase
    {
        /// <summary>
        ///     Validates last name.
        /// </summary>
        /// <param name="record">Object to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown when lastname is null or empty or whitespace.</exception>
        /// <exception cref="ArgumentException">Thrown when lastname's length is less than 2 or greater than 60 or value contains non letters chars.</exception>
        public override void Validate(FileCabinetRecord record)
        {
            Guard.ArgumentIsNotNull(record.LastName, nameof(record.LastName));
            Guard.ArgumentIsNotEmptyOrWhiteSpace(record.LastName, nameof(record.LastName));
            Guard.ArgumentMatchRegex(record.LastName, nameof(record.LastName), @"^[A-Za-z]{2,60}$");
        }
    }
}
