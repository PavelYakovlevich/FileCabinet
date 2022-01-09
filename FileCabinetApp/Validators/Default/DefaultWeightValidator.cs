using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Default weight validator.
    /// </summary>
    public class DefaultWeightValidator : FileRecordValidatorBase
    {
        /// <summary>
        ///     Validates weight.
        /// </summary>
        /// <param name="record">Object to validate.</param>
        /// <exception cref="ArgumentException">Thrown when weight is less than 1.</exception>
        public override void Validate(FileCabinetRecord record)
        {
            Guard.ArgumentGreaterThan(record.Weight, 0);
        }
    }
}
