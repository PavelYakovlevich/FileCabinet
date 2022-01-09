using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Custom gender validator.
    /// </summary>
    public class CustomGenderValidator : FileRecordValidatorBase
    {
        /// <summary>
        ///     Validates gender.
        /// </summary>
        /// <param name="record">Object to validate.</param>
        /// <exception cref="ArgumentException">Thrown when gender is not equal to M or F.</exception>
        public override void Validate(FileCabinetRecord record)
        {
            Guard.ArgumentIsInRange(record.Gender, new[] { 'M', 'F' });
        }
    }
}
