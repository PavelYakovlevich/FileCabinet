using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    /// <summary>
    ///     Class for the list of validation functions.
    /// </summary>
    /// <typeparam name="T">
    ///     Type of the value, which must be validated.
    /// </typeparam>
    public class ParameterValidationChain<T>
    {
        private List<Predicate<T>> validationList = new List<Predicate<T>>();

        /// <summary>
        ///     Validates the specified value.
        /// </summary>
        /// <param name="value">
        ///     Contains function, which converts input string to value with type <typeparamref name="T"/>.
        /// </param>
        /// <returns>Converted value of type <typeparamref name="T"/>.</returns>
        public bool Validate(T value)
        {
            bool valueIsValid = true;

            foreach (var validationFunc in this.validationList)
            {
                valueIsValid = valueIsValid && validationFunc(value);
            }

            return valueIsValid;
        }

        /// <summary>
        ///     Adds new validation function to the validation function list.
        /// </summary>
        /// <param name="condition">
        ///     <see cref="Predicate{T}"/> function, which will be added to the validation function list.
        /// </param>
        /// <returns>Converted value of type <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="condition"/> is null.</exception>
        public ParameterValidationChain<T> AddCondition(Predicate<T> condition)
        {
            Guard.ArgumentIsNotNull(condition, nameof(condition));

            this.validationList.Add(condition);

            return this;
        }
    }
}