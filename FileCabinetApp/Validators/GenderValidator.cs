using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Gender parametrized validator.
    /// </summary>
    public class GenderValidator : FileRecordValidatorBase
    {
        private readonly char[] genderValues;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GenderValidator"/> class.
        /// </summary>
        /// <param name="genderValues">Values, which gender can have.</param>
        /// <exception cref="ArgumentNullException">Trown when <paramref name="genderValues"/> is null.</exception>
        /// <exception cref="ArgumentException">Trown when <paramref name="genderValues"/> is empty.</exception>
        public GenderValidator(char[] genderValues)
        {
            Guard.ArgumentIsNotNull(genderValues, nameof(genderValues));
            Guard.ArgumentGreaterThan(genderValues.Length, 0, $"{nameof(genderValues)} is empty.");

            this.genderValues = genderValues;
        }

        /// <summary>
        ///     Validates gender.
        /// </summary>
        /// <param name="record">Object to validate.</param>
        /// <exception cref="ArgumentException">Thrown when gender is in values list.</exception>
        public override void Validate(FileCabinetRecord record)
        {
            Guard.ArgumentIsInRange(record.Gender, this.genderValues);
        }
    }
}