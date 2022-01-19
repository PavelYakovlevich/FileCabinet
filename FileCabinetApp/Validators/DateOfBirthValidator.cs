using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Date of birth parametrized validator.
    /// </summary>
    public class DateOfBirthValidator : FileRecordValidatorBase
    {
        private readonly DateTime from;
        private readonly DateTime to;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DateOfBirthValidator"/> class.
        /// </summary>
        /// <param name="from">Minimal acceptable date.</param>
        /// <param name="to">Maximum acceptable date.</param>
        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            this.from = from;
            this.to = to;
        }

        /// <summary>
        ///     Validates date of birth.
        /// </summary>
        /// <param name="record">Object to validate.</param>
        /// <exception cref="ArgumentException">Thrown when date of birth is less than minimal date or greater than maximum date.</exception>
        public override void Validate(FileCabinetRecord record)
        {
            Guard.ArgumentIsNotNull(record, nameof(record));

            Guard.ArgumentSatisfies(
                record.DateOfBirth,
                (dateOfBirth) => dateOfBirth.CompareTo(this.from) >= 0 && dateOfBirth.CompareTo(this.to) <= 0,
                $"{nameof(record.DateOfBirth)} must be greater than {this.from.ToString("MM/dd/yyyy")} and less or equal to {this.to.ToString("MM/dd/yyyy")}.");
        }
    }
}