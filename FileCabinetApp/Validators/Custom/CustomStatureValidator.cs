using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Custom stature validator.
    /// </summary>
    public class CustomStatureValidator : FileRecordValidatorBase
    {
        /// <summary>
        ///     Validates stature.
        /// </summary>
        /// <param name="record">Object to validate.</param>
        /// <exception cref="ArgumentException">Thrown when stature is less than 1 or greater than 300.</exception>
        public override void Validate(FileCabinetRecord record)
        {
            Guard.ArgumentGreaterThan(record.Stature, 0);
            Guard.ArgumentLessThan(record.Stature, 300);
        }
    }
}
