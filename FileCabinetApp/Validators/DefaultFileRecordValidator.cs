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
            Guard.ArgumentIsNotNull(parameterObject, nameof(parameterObject));

            Guard.ArgumentIsNotNull(parameterObject.FirstName, nameof(parameterObject.FirstName));
            Guard.ArgumentIsNotEmptyOrWhiteSpace(parameterObject.FirstName, nameof(parameterObject.FirstName));
            Guard.ArgumentSatisfies(
                parameterObject.FirstName,
                (firstName) => firstName.Length >= 2 && firstName.Length <= 60,
                $"{nameof(parameterObject.FirstName)} lenght must be greater than 1 and less than 61.");

            Guard.ArgumentIsNotNull(parameterObject.LastName, nameof(parameterObject.LastName));
            Guard.ArgumentIsNotEmptyOrWhiteSpace(parameterObject.LastName, nameof(parameterObject.LastName));
            Guard.ArgumentSatisfies(
                parameterObject.LastName,
                (lastName) => lastName.Length >= 2 && lastName.Length <= 60,
                $"{nameof(parameterObject.LastName)} lenght must be greater than 1 and less than 61.");

            Guard.ArgumentSatisfies(
                parameterObject.DateOfBirth,
                (dateOfBirth) => dateOfBirth.CompareTo(new DateTime(1950, 1, 1)) >= 0 && dateOfBirth.CompareTo(DateTime.Now) <= 0,
                $"{nameof(parameterObject.DateOfBirth)} must be greater than 01-Jan-1950 and less or equal to current date.");

            Guard.ArgumentIsInRange(parameterObject.Gender, new[] { 'M', 'F' });

            Guard.ArgumentGreaterThan(parameterObject.Weight, 0);

            Guard.ArgumentGreaterThan(parameterObject.Stature, 0);
        }
    }
}
