using System;

namespace FileCabinetApp.Configuration
{
    /// <summary>
    ///     Class for <see cref="DateTime"/> constraint info.
    /// </summary>
    /// <typeparam name="T">Numeric type of constraint's value.</typeparam>
    public class NumericRangeConstraint<T>
        where T : struct
    {
        /// <summary>
        ///     Gets or sets minimal value.
        /// </summary>
        /// <value>Minimal value.</value>
        public T MinValue { get; set; }

        /// <summary>
        ///     Gets or sets maximum value.
        /// </summary>
        /// <value>Maximum value.</value>
        public T MaxValue { get; set; }
    }
}