using System;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    ///     Class for the input arguments, which are passed from console.
    /// </summary>
    internal class ProgramInputArgument
    {
        private string defaultValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramInputArgument"/> class.
        /// </summary>
        /// <param name="name">Name of the parameter.</param>
        /// <param name="abbreviation">Shortcut fo the parameter's name.</param>
        /// <param name="values">Values, which can be specified.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> or <paramref name="abbreviation"/> or <paramref name="values"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> or <paramref name="abbreviation"/> is empty or whitespace or <paramref name="values"/> is empty.</exception>
        internal ProgramInputArgument(string name, string abbreviation, string[] values)
        {
            Guard.ArgumentIsNotNull(name, nameof(name));
            Guard.ArgumentIsNotEmptyOrWhiteSpace(name, nameof(name));

            Guard.ArgumentIsNotNull(abbreviation, nameof(abbreviation));
            Guard.ArgumentIsNotEmptyOrWhiteSpace(abbreviation, nameof(abbreviation));

            Guard.ArgumentIsNotNull(values, nameof(values));
            Guard.ArgumentSatisfies(values, (values) => values.Length > 0, $"{nameof(values)} must have at least one element!");

            this.Name = name;
            this.Abbreviation = abbreviation;
            this.ValidValues = values;
            this.defaultValue = values[0];
        }

        /// <summary>
        ///     Gets argument's name.
        /// </summary>
        /// <value>Argument's name.</value>
        internal string Name { get; }

        /// <summary>
        ///     Gets argument's name abbreviation.
        /// </summary>
        /// <value>Argument's name abbreviation.</value>
        internal string Abbreviation { get; }

        /// <summary>
        ///     Gets valid values for the argument.
        /// </summary>
        /// <value>Argument's name abbreviation.</value>
        internal string[] ValidValues { get; }

        /// <summary>
        ///     Checks if <paramref name="value"/> is valid for the argument.
        /// </summary>
        /// <param name="value">Checkable value.</param>
        /// <returns>true if values is valid, overwise false.</returns>
        internal bool ValidateValue(string value)
        {
            foreach (var validValue in this.ValidValues)
            {
                if (validValue.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Builds <see cref="string"/>, which contains all valid values, separated by the comma.
        /// </summary>
        /// <returns><see cref="string"/>, which contains all valid values, separated by the comma.</returns>
        internal string GetValidValuesString()
        {
            StringBuilder result = new StringBuilder();

            foreach (var validValue in this.ValidValues)
            {
                result.Append($"{validValue},");
            }

            return result.Remove(result.Length - 1, 1).ToString();
        }
    }
}
