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
            Guard.ArgumentIsNotNull(parameterObject, nameof(parameterObject));

            Guard.ArgumentIsNotNull(parameterObject.FirstName, nameof(parameterObject.FirstName));
            Guard.ArgumentIsNotEmptyOrWhiteSpace(parameterObject.FirstName, nameof(parameterObject.FirstName));
            Guard.ArgumentMatchRegex(parameterObject.FirstName, nameof(parameterObject.FirstName), @"^[A-Za-z]{2,60}$");

            Guard.ArgumentIsNotNull(parameterObject.LastName, nameof(parameterObject.LastName));
            Guard.ArgumentIsNotEmptyOrWhiteSpace(parameterObject.LastName, nameof(parameterObject.LastName));
            Guard.ArgumentMatchRegex(parameterObject.LastName, nameof(parameterObject.LastName), @"^[A-Za-z]{2,60}$");

            Guard.ArgumentSatisfies(
                parameterObject.DateOfBirth,
                (dateOfBirth) => dateOfBirth.CompareTo(new DateTime(1950, 1, 1)) >= 0 && dateOfBirth.CompareTo(DateTime.Now) <= 0,
                $"{nameof(parameterObject.DateOfBirth)} must be greater than 01-Jan-1950 and less or equal to current date.");

            Guard.ArgumentIsInRange(parameterObject.Gender, new[] { 'M', 'F' });

            Guard.ArgumentGreaterThan(parameterObject.Weight, 0);
            Guard.ArgumentLessThan(parameterObject.Weight, 1000.0m);

            Guard.ArgumentGreaterThan(parameterObject.Stature, 0);
            Guard.ArgumentLessThan(parameterObject.Stature, 300);
        }
    }
}
