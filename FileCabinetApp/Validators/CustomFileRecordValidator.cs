using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Custom validator for the <see cref="FileCabinetRecordParameterObject"/> object.
    /// </summary>
    internal class CustomFileRecordValidator : FileRecordValidatorBase
    {
        /// <inheritdoc cref="FileRecordValidatorBase.Validate(FileCabinetRecord)"/>
        public override void Validate(FileCabinetRecord record)
        {
            Guard.ArgumentIsNotNull(record, nameof(record));

            this.ValidateFirstName(record);
            this.ValidateLastName(record);
            this.ValidateDateOfBirth(record);
            this.ValidateGender(record);
            this.ValidateWeight(record);
            this.ValidateStature(record);
        }

        private void ValidateFirstName(FileCabinetRecord record)
        {
            Guard.ArgumentIsNotNull(record.FirstName, nameof(record.FirstName));
            Guard.ArgumentIsNotEmptyOrWhiteSpace(record.FirstName, nameof(record.FirstName));
            Guard.ArgumentMatchRegex(record.FirstName, nameof(record.FirstName), @"^[A-Za-z]{2,60}$");
        }

        private void ValidateLastName(FileCabinetRecord record)
        {
            Guard.ArgumentIsNotNull(record.LastName, nameof(record.LastName));
            Guard.ArgumentIsNotEmptyOrWhiteSpace(record.LastName, nameof(record.LastName));
            Guard.ArgumentMatchRegex(record.LastName, nameof(record.LastName), @"^[A-Za-z]{2,60}$");
        }

        private void ValidateDateOfBirth(FileCabinetRecord record)
        {
            Guard.ArgumentSatisfies(
                record.DateOfBirth,
                (dateOfBirth) => dateOfBirth.CompareTo(new DateTime(1950, 1, 1)) >= 0 && dateOfBirth.CompareTo(DateTime.Now) <= 0,
                $"{nameof(record.DateOfBirth)} must be greater than 01-Jan-1950 and less or equal to current date.");
        }

        private void ValidateGender(FileCabinetRecord record)
        {
            Guard.ArgumentIsInRange(record.Gender, new[] { 'M', 'F' });
        }

        private void ValidateWeight(FileCabinetRecord record)
        {
            Guard.ArgumentGreaterThan(record.Weight, 0);
            Guard.ArgumentLessThan(record.Weight, 1000.0m);
        }

        private void ValidateStature(FileCabinetRecord record)
        {
            Guard.ArgumentGreaterThan(record.Stature, 0);
            Guard.ArgumentLessThan(record.Stature, 300);
        }
    }
}
