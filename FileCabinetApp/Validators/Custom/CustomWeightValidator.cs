using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Custom weight validator.
    /// </summary>
    public class CustomWeightValidator : FileRecordValidatorBase
    {
        /// <summary>
        ///     Validates weight.
        /// </summary>
        /// <param name="record">Object to validate.</param>
        /// <exception cref="ArgumentException">Thrown when weight is less than 1 or greater than 1000.</exception>
        public override void Validate(FileCabinetRecord record)
        {
            Guard.ArgumentGreaterThan(record.Weight, 0);
            Guard.ArgumentLessThan(record.Weight, 1000.0m);
        }
    }
}
