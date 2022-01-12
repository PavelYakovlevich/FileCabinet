#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace FileCabinetApp.Configuration
{
    /// <summary>
    ///     Data class for validation rules configuration.
    /// </summary>
    public class ValidationConfig
    {
        /// <summary>
        ///     Gets or sets first name configuration info.
        /// </summary>
        /// <value>Firstname configuration info.</value>
        public StringConstraint FirstName { get; set; }

        /// <summary>
        ///     Gets or sets last name configuration info.
        /// </summary>
        /// <value>Lastname configuration info.</value>
        public StringConstraint LastName { get; set; }

        /// <summary>
        ///     Gets or sets date of birth configuration info.
        /// </summary>
        /// <value>Date of birth configuration info.</value>
        public DateTimeRangeConstraint DateOfBirth { get; set; }

        /// <summary>
        ///     Gets or sets stature configuration info.
        /// </summary>
        /// <value>Stature configuration info.</value>
        public NumericRangeConstraint<short> Stature { get; set; }

        /// <summary>
        ///     Gets or sets weight configuration info.
        /// </summary>
        /// <value>Weight configuration info.</value>
        public NumericRangeConstraint<decimal> Weight { get; set; }

        /// <summary>
        ///     Gets or sets allowed gender values.
        /// </summary>
        /// <value>Allowed gender values.</value>
        public char[] Gender { get; set; }
    }
}
