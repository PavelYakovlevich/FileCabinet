using System;

namespace ProgramInputHandling
{
    /// <summary>
    ///     Abstract class for all console input paramters.
    /// </summary>
    public abstract class AbstractInputParamter : IInputParameter
    {
        private string name;
        private string abbreviation;
        private bool isMandatory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AbstractInputParamter"/> class.
        /// </summary>
        /// <param name="name">Name of the parameter.</param>
        /// <param name="abbreviation">Shortcut fo the parameter's name.</param>
        /// <param name="isMandatory">Specifies if paramter is mandatory or not.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> or <paramref name="abbreviation"/> is empty or whitespace or null.</exception>
        protected AbstractInputParamter(string name, string abbreviation, bool isMandatory)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), $"'{nameof(name)}' cannot be null or empty or whitespace.");
            }

            if (string.IsNullOrEmpty(abbreviation) || string.IsNullOrWhiteSpace(abbreviation))
            {
                throw new ArgumentNullException(nameof(abbreviation), $"'{nameof(abbreviation)}' cannot be null or empty or whitespace.");
            }

            this.name = name;
            this.abbreviation = abbreviation;
            this.isMandatory = isMandatory;
        }

        /// <inheritdoc cref="IInputParameter.Name"/>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <inheritdoc cref="IInputParameter.Abbreviation"/>
        public string Abbreviation
        {
            get
            {
                return this.abbreviation;
            }
        }

        /// <inheritdoc cref="IInputParameter.IsMandatory"/>
        public bool IsMandatory
        {
            get
            {
                return this.isMandatory;
            }
        }

        /// <inheritdoc cref="IInputParameter.ValidateValue(string)"/>
        public abstract bool ValidateValue(string value);
    }
}
