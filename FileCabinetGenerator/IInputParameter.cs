namespace FileCabinetGenerator
{
    /// <summary>
    ///     Interface for all console input paramters.
    /// </summary>
    public interface IInputParameter
    {
        /// <summary>
        ///     Gets argument's name.
        /// </summary>
        /// <value>Argument's name.</value>
        public string Name { get; }

        /// <summary>
        ///     Gets argument's name abbreviation.
        /// </summary>
        /// <value>Argument's name abbreviation.</value>
        public string Abbreviation { get; }

        /// <summary>
        ///     Gets a value indicating whethere argument is mandatory.
        /// </summary>
        /// <value>true if argument is mandatory, overwise false.</value>
        public bool IsMandatory { get; }

        /// <summary>
        ///     Checks if <paramref name="value"/> is valid for the argument.
        /// </summary>
        /// <param name="value">Checkable value.</param>
        /// <returns>true if values is valid, overwise false.</returns>
        public bool ValidateValue(string value);
    }
}
