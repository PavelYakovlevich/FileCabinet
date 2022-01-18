using System;

namespace FileCabinetApp.Configuration
{
    /// <summary>
    ///     Class for <see cref="DateTime"/> constraint info.
    /// </summary>
    public class DateTimeRangeConstraint
    {
        /// <summary>
        ///     Gets or sets minimal date's value.
        /// </summary>
        /// <value>Minimal date's value.</value>
        public DateTime From { get; set; }

        /// <summary>
        ///     Gets or sets maxinun date's value.
        /// </summary>
        /// <value>Maximum date's value.</value>
        public DateTime To { get; set; }
    }
}