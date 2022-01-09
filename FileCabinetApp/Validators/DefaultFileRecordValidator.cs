using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Default validator for the <see cref="FileCabinetRecordParameterObject"/> object.
    /// </summary>
    internal class DefaultFileRecordValidator : FileRecordValidatorBase
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
            Guard.ArgumentSatisfies(
                record.FirstName,
                (firstName) => firstName.Length >= 2 && firstName.Length <= 60,
                $"{nameof(record.FirstName)} lenght must be greater than 1 and less than 61.");
        }

        private void ValidateLastName(FileCabinetRecord record)
        {
            Guard.ArgumentIsNotNull(record.LastName, nameof(record.LastName));
            Guard.ArgumentIsNotEmptyOrWhiteSpace(record.LastName, nameof(record.LastName));
            Guard.ArgumentSatisfies(
                record.LastName,
                (lastName) => lastName.Length >= 2 && lastName.Length <= 60,
                $"{nameof(record.LastName)} lenght must be greater than 1 and less than 61.");
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
        }

        private void ValidateStature(FileCabinetRecord record)
        {
            Guard.ArgumentGreaterThan(record.Stature, 0);
        }
    }
}
