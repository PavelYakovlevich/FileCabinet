using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ProgramInputHandling
{
    /// <summary>
    ///     Class for a generic value parameter.
    /// </summary>
    public class ValueInputParameter : AbstractInputParameter
    {
        private static readonly Dictionary<ArgumentType, Predicate<string>> ValidationFunctions = new Dictionary<ArgumentType, Predicate<string>>()
        {
            { ArgumentType.Integer, (value) => new Regex(@"^[\-\+]{0,1}\d+$").IsMatch(value) },
            { ArgumentType.PositiveInteger, (value) => new Regex(@"^\+{0,1}\d+$").IsMatch(value) },
            { ArgumentType.FilePath, (value) => new Regex(@"^[a-zA-Z]:[\\\/](?:[a-zA-Z\d]+[\\\/])*([a-zA-Z\d]+\.[A-Za-z]+)$").IsMatch(value) },
        };

        private Predicate<string> validationFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueInputParameter"/> class.
        /// </summary>
        /// <param name="name">Name of the parameter.</param>
        /// <param name="abbreviation">Shortcut fo the parameter's name.</param>
        /// <param name="isMandatory">Specifies if paramter is mandatory or not.</param>
        /// <param name="argumentType">Specifies type of arguments value.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> or <paramref name="abbreviation"/> is empty or whitespace or null.</exception>
        public ValueInputParameter(string name, string abbreviation, bool isMandatory, ArgumentType argumentType)
            : base(name, abbreviation, isMandatory)
        {
            if (!ValidationFunctions.ContainsKey(argumentType))
            {
                throw new NotSupportedException($"{argumentType} is not supported.");
            }

            this.ValueType = argumentType;
            this.validationFunc = ValidationFunctions[argumentType];
        }

        /// <summary>
        /// Gets type of value of input parameter.
        /// </summary>
        /// <value>Type of value of input parameter.</value>
        public ArgumentType ValueType { get; }

        /// <inheritdoc cref="AbstractInputParamter.ValidateValue(string)"/>
        public override bool ValidateValue(string value)
        {
            return this.validationFunc(value);
        }
    }
}
