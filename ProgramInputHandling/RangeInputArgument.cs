using System;
using System.Text;

namespace ProgramInputHandling
{
    /// <summary>
    ///     Class for the input arguments, which are passed from console.
    /// </summary>
    public class RangeInputArgument : AbstractInputParamter
    {
        private string defaultValue;

        /// <inheritdoc cref="AbstractInputParamter"/>
        /// <param name="values">Values, which can be specified.</param>
        public RangeInputArgument(string name, string abbreviation, bool isMandatory, string[] values)
            : base(name, abbreviation, isMandatory)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values), $"{values} can't be null.");
            }

            if (values.Length == 0)
            {
                throw new ArgumentException($"{values} can't be empty.", nameof(values));
            }

            this.ValidValues = values;
            this.defaultValue = values[0];
        }

        /// <summary>
        ///     Gets valid values for the argument.
        /// </summary>
        /// <value>Argument's name abbreviation.</value>
        public string[] ValidValues { get; }

        /// <summary>
        ///     Builds <see cref="string"/>, which contains all valid values, separated by the comma.
        /// </summary>
        /// <returns><see cref="string"/>, which contains all valid values, separated by the comma.</returns>
        public string GetValidValuesString()
        {
            StringBuilder result = new StringBuilder();

            foreach (var validValue in this.ValidValues)
            {
                result.Append($"{validValue},");
            }

            return result.Remove(result.Length - 1, 1).ToString();
        }

        /// <inheritdoc cref="AbstractInputParamter.ValidateValue(string)"/>
        public override bool ValidateValue(string value)
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
    }
}
