using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Default validator for the <see cref="FileCabinetRecordParameterObject"/> object.
    /// </summary>
    internal class DefaultFileRecordValidator : FileRecordValidatorBase
    {
        /// <inheritdoc cref="FileRecordValidatorBase.Validate(FileCabinetRecord)"/>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="record"/> is null.</exception>
        public override void Validate(FileCabinetRecord record)
        {
            Guard.ArgumentIsNotNull(record, nameof(record));

            new FirstNameValidator(2, 60).Validate(record);
            new LastNameValidator(2, 60).Validate(record);
            new DateOfBirthValidator(new DateTime(1950, 1, 1), DateTime.Now).Validate(record);
            new DefaultWeightValidator().Validate(record);
            new DefaultGenderValidator().Validate(record);
            new DefaultStatureValidator().Validate(record);
        }
    }
}
