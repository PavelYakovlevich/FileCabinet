using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Custom validator for the <see cref="FileCabinetRecordParameterObject"/> object.
    /// </summary>
    internal class CustomFileRecordValidator : IRecordValidator
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
            Guard.ArgumentMatchRegex(record.FirstName, nameof(record.FirstName), @"^[A-Za-z]{2,60}$");

            Guard.ArgumentIsNotNull(record.LastName, nameof(record.LastName));
            Guard.ArgumentIsNotEmptyOrWhiteSpace(record.LastName, nameof(record.LastName));
            Guard.ArgumentMatchRegex(record.LastName, nameof(record.LastName), @"^[A-Za-z]{2,60}$");

            Guard.ArgumentSatisfies(
                record.DateOfBirth,
                (dateOfBirth) => dateOfBirth.CompareTo(new DateTime(1950, 1, 1)) >= 0 && dateOfBirth.CompareTo(DateTime.Now) <= 0,
                $"{nameof(record.DateOfBirth)} must be greater than 01-Jan-1950 and less or equal to current date.");

            Guard.ArgumentIsInRange(record.Gender, new[] { 'M', 'F' });

            Guard.ArgumentGreaterThan(record.Weight, 0);
            Guard.ArgumentLessThan(record.Weight, 1000.0m);

            Guard.ArgumentGreaterThan(record.Stature, 0);
            Guard.ArgumentLessThan(record.Stature, 300);
        }
    }
}
