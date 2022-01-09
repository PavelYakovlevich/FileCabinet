using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Stature parametrized validator.
    /// </summary>
    public class StatureValidator : FileRecordValidatorBase
    {
        private readonly int minValue;
        private readonly int maxValue;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StatureValidator"/> class.
        /// </summary>
        /// <param name="min">Minimal value of stature.</param>
        public StatureValidator(int min)
        {
            this.minValue = min;
            this.maxValue = int.MaxValue;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StatureValidator"/> class.
        /// </summary>
        /// <param name="min">Minimal value of stature.</param>
        /// <param name="max">Maximum value of stature.</param>
        public StatureValidator(int min, int max)
            : this(min)
        {
            this.maxValue = max;
        }

        /// <summary>
        ///     Validates stature.
        /// </summary>
        /// <param name="record">Object to validate.</param>
        /// <exception cref="ArgumentException">Thrown when stature is less than minimal value or greater than maximum value.</exception>
        public override void Validate(FileCabinetRecord record)
        {
            Guard.ArgumentGreaterThan(record.Stature, this.minValue);
            Guard.ArgumentLessThan(record.Stature, this.maxValue);
        }
    }
}
