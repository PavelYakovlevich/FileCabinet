using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Default first name validator.
    /// </summary>
    public class DefaultFirstNameValidator : FileRecordValidatorBase
    {
        /// <summary>
        ///     Validates first name.
        /// </summary>
        /// <param name="record">Object to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown when firstname is null or empty or whitespace.</exception>
        /// <exception cref="ArgumentException">Thrown when firstname's length is less than 2 or greater than 60.</exception>
        public override void Validate(FileCabinetRecord record)
        {
            Guard.ArgumentIsNotNull(record.FirstName, nameof(record.FirstName));
            Guard.ArgumentIsNotEmptyOrWhiteSpace(record.FirstName, nameof(record.FirstName));
            Guard.ArgumentSatisfies(
                record.FirstName,
                (firstName) => firstName.Length >= 2 && firstName.Length <= 60,
                $"{nameof(record.FirstName)} lenght must be greater than 1 and less than 61.");
        }
    }
}
