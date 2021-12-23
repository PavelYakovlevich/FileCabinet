using System;

namespace FileCabinetApp.Services
{
    /// <summary>
    ///     Class for the custom implementaion of <see cref="FileCabinetService"/>.
    /// </summary>
    internal class FileCabinetCustomService : FileCabinetService
    {
        /// <inheritdoc cref="FileCabinetService.ValidateParameters(FileCabinetRecordParameterObject)"/>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameterObject"/> is null or <paramref name="parameterObject"/>'s firstName or lastName is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Throw when <paramref name="parameterObject"/> inner properties are not valid:
        ///         - firstName of lastName is empty or whitespace;
        ///         - firstName's or lastName's length is lower less than 2 or greater than 60 or contains at least one not letter character;
        ///         - dateOfBirth is less than 1-Jan-1950 or greater than todays date;
        ///         - weight is lower than 0;
        ///         - stature is lower than 0;
        ///         - file cabinet record with specified id not exists.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="parameterObject"/>'s gender is not equal to M or F.</exception>
        protected override void ValidateParameters(FileCabinetRecordParameterObject parameterObject)
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

            Guard.ArgumentGreaterThan(parameterObject.Stature, 0);
        }
    }
}
