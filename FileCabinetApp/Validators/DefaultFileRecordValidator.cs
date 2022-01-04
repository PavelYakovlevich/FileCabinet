using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Default validator for the <see cref="FileCabinetRecordParameterObject"/> object.
    /// </summary>
    internal class DefaultFileRecordValidator : IRecordValidator
    {
        /// <inheritdoc cref="IRecordValidator.ValidateParameters(FileCabinetRecordParameterObject)"/>
        public void ValidateParameters(FileCabinetRecordParameterObject parameterObject)
        {
            var record = new FileCabinetRecord
            {
                Id = parameterObject.Id,
                FirstName = parameterObject.FirstName,
                LastName = parameterObject.LastName,
                DateOfBirth = parameterObject.DateOfBirth,
                Gender = parameterObject.Gender,
                Weight = parameterObject.Weight,
                Stature = parameterObject.Stature,
            };

            this.Validate(record);
        }

        /// <inheritdoc cref="IRecordValidator.Validate(FileCabinetRecord)"/>
        public void Validate(FileCabinetRecord record)
        {
            Guard.ArgumentIsNotNull(record, nameof(record));

            Guard.ArgumentIsNotNull(record.FirstName, nameof(record.FirstName));
            Guard.ArgumentIsNotEmptyOrWhiteSpace(record.FirstName, nameof(record.FirstName));
            Guard.ArgumentSatisfies(
                record.FirstName,
                (firstName) => firstName.Length >= 2 && firstName.Length <= 60,
                $"{nameof(record.FirstName)} lenght must be greater than 1 and less than 61.");

            Guard.ArgumentIsNotNull(record.LastName, nameof(record.LastName));
            Guard.ArgumentIsNotEmptyOrWhiteSpace(record.LastName, nameof(record.LastName));
            Guard.ArgumentSatisfies(
                record.LastName,
                (lastName) => lastName.Length >= 2 && lastName.Length <= 60,
                $"{nameof(record.LastName)} lenght must be greater than 1 and less than 61.");

            Guard.ArgumentSatisfies(
                record.DateOfBirth,
                (dateOfBirth) => dateOfBirth.CompareTo(new DateTime(1950, 1, 1)) >= 0 && dateOfBirth.CompareTo(DateTime.Now) <= 0,
                $"{nameof(record.DateOfBirth)} must be greater than 01-Jan-1950 and less or equal to current date.");

            Guard.ArgumentIsInRange(record.Gender, new[] { 'M', 'F' });

            Guard.ArgumentGreaterThan(record.Weight, 0);

            Guard.ArgumentGreaterThan(record.Stature, 0);
        }
    }
}
