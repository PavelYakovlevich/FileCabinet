using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Default stature validator.
    /// </summary>
    public class DefaultStatureValidator : FileRecordValidatorBase
    {
        /// <summary>
        ///     Validates stature.
        /// </summary>
        /// <param name="record">Object to validate.</param>
        /// <exception cref="ArgumentException">Thrown when stature is less than 1.</exception>
        public override void Validate(FileCabinetRecord record)
        {
            Guard.ArgumentGreaterThan(record.Stature, 0);
        }
    }
}
