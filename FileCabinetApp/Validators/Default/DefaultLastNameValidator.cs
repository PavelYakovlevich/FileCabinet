using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Default last name validator.
    /// </summary>
    public class DefaultLastNameValidator : FileRecordValidatorBase
    {
        /// <summary>
        ///     Validates last name.
        /// </summary>
        /// <param name="record">Object to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown when lastname is null or empty or whitespace.</exception>
        /// <exception cref="ArgumentException">Thrown when lastname's length is less than 2 or greater than 60.</exception>
        public override void Validate(FileCabinetRecord record)
        {
            Guard.ArgumentIsNotNull(record.LastName, nameof(record.LastName));
            Guard.ArgumentIsNotEmptyOrWhiteSpace(record.LastName, nameof(record.LastName));
            Guard.ArgumentSatisfies(
                record.LastName,
                (lastName) => lastName.Length >= 2 && lastName.Length <= 60,
                $"{nameof(record.LastName)} lenght must be greater than 1 and less than 61.");
        }
    }
}
