using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ProgramInputHandling
{
    public class ValueInputParameter : AbstractInputParameter
    {
        private static readonly Dictionary<ArgumentType, Predicate<string>> ValidationFunctions = new Dictionary<ArgumentType, Predicate<string>>()
        {
            { ArgumentType.Integer, (value) => new Regex(@"^[\-\+]{0,1}\d+$").IsMatch(value) },
            { ArgumentType.PositiveInteger, (value) => new Regex(@"^\+{0,1}\d+$").IsMatch(value) },
            { ArgumentType.FilePath, (value) => new Regex(@"^[a-zA-Z]:[\\\/](?:[a-zA-Z\d]+[\\\/])*([a-zA-Z\d]+\.[A-Za-z]+)$").IsMatch(value) },
        };

        private Predicate<string> validationFunc;

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
