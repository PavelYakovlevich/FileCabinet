using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Custom date of birth validator.
    /// </summary>
    public class CustomDateOfBirthValidator : FileRecordValidatorBase
    {
        /// <summary>
        ///     Validates date of birth.
        /// </summary>
        /// <param name="record">Object to validate.</param>
        /// <exception cref="ArgumentException">Thrown when date of birth is less than 1/1/1950 or greater than today's date.</exception>
        public override void Validate(FileCabinetRecord record)
        {
            Guard.ArgumentSatisfies(
                record.DateOfBirth,
                (dateOfBirth) => dateOfBirth.CompareTo(new DateTime(1950, 1, 1)) >= 0 && dateOfBirth.CompareTo(DateTime.Now) <= 0,
                $"{nameof(record.DateOfBirth)} must be greater than 01-Jan-1950 and less or equal to current date.");
        }
    }
}
