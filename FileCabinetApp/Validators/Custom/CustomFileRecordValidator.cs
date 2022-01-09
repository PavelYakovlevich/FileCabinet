using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Custom validator for the <see cref="FileCabinetRecordParameterObject"/> object.
    /// </summary>
    internal class CustomFileRecordValidator : FileRecordValidatorBase
    {
        /// <inheritdoc cref="FileRecordValidatorBase.Validate(FileCabinetRecord)"/>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="record"/> is null.</exception>
        public override void Validate(FileCabinetRecord record)
        {
            Guard.ArgumentIsNotNull(record, nameof(record));

            new CustomFirstNameValidator().Validate(record);
            new CustomLastNameValidator().Validate(record);
            new CustomDateOfBirthValidator().Validate(record);
            new CustomGenderValidator().Validate(record);
            new CustomWeightValidator().Validate(record);
            new CustomStatureValidator().Validate(record);
        }
    }
}
