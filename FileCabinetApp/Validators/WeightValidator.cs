using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Weight parametrized validator.
    /// </summary>
    public class WeightValidator : FileRecordValidatorBase
    {
        private decimal minValue;
        private decimal maxValue;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WeightValidator"/> class.
        /// </summary>
        /// <param name="minValue">Minimal value of weight.</param>
        public WeightValidator(decimal minValue)
        {
            this.minValue = minValue;
            this.maxValue = decimal.MaxValue;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WeightValidator"/> class.
        /// </summary>
        /// <param name="minValue">Minimal value of weight.</param>
        /// <param name="maxValue">Maximum value of weight.</param>
        public WeightValidator(decimal minValue, decimal maxValue)
            : this(minValue)
        {
            this.maxValue = maxValue;
        }

        /// <summary>
        ///     Validates weight.
        /// </summary>
        /// <param name="record">Object to validate.</param>
        /// <exception cref="ArgumentException">Thrown when weight is less than minimal value or greater than max value.</exception>
        public override void Validate(FileCabinetRecord record)
        {
            Guard.ArgumentGreaterThan(record.Weight, this.minValue);
            Guard.ArgumentLessThan(record.Weight, this.maxValue);
        }
    }
}
