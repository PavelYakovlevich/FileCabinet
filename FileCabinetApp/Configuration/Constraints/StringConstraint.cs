namespace FileCabinetApp.Configuration
{
    /// <summary>
    ///     Class for <see cref="string"/> constraint info.
    /// </summary>
    public class StringConstraint
    {
        /// <summary>
        ///     Gets or sets minimal string's length.
        /// </summary>
        /// <value>Minimal string's length.</value>
        public int MinLength { get; set; } = 0;

        /// <summary>
        ///     Gets or sets maximum string's length.
        /// </summary>
        /// <value>Maximum string's length.</value>
        public int MaxLength { get; set; } = int.MaxValue;
    }
}
