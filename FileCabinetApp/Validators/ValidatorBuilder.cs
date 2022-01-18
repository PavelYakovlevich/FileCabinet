using System;
using System.Collections.Generic;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Class for building a <see cref="FileCabinetRecord"/> validator.
    /// </summary>
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators = new List<IRecordValidator>();

        /// <summary>
        ///     Adds <see cref="FirstNameValidator"/> into the validation chain.
        /// </summary>
        /// <param name="minLength">Minimal length of the first name.</param>
        /// <param name="maxLength">Maximum length of the first name.</param>
        /// <returns>Current <see cref="ValidatorBuilder"/> object.</returns>
        public ValidatorBuilder ValidateFirstName(int minLength, int maxLength)
        {
            this.validators.Add(new FirstNameValidator(minLength, maxLength));

            return this;
        }

        /// <summary>
        ///     Adds <see cref="LastNameValidator"/> into the validation chain.
        /// </summary>
        /// <param name="minLength">Minimal length of the last name.</param>
        /// <param name="maxLength">Maximum length of the last name.</param>
        /// <returns>Current <see cref="ValidatorBuilder"/> object.</returns>
        public ValidatorBuilder ValidateLastName(int minLength, int maxLength)
        {
            this.validators.Add(new LastNameValidator(minLength, maxLength));

            return this;
        }

        /// <summary>
        ///     Adds <see cref="DateOfBirthValidator"/> into the validation chain.
        /// </summary>
        /// <param name="from">Minimal acceptable date.</param>
        /// <param name="to">Maximum acceptable date.</param>
        /// <returns>Current <see cref="ValidatorBuilder"/> object.</returns>
        public ValidatorBuilder ValidateDateOfBirth(DateTime from, DateTime to)
        {
            this.validators.Add(new DateOfBirthValidator(from, to));

            return this;
        }

        /// <summary>
        ///     Adds <see cref="StatureValidator"/> into the validation chain.
        /// </summary>
        /// <param name="minValue">Minimal value of stature.</param>
        /// <param name="maxValue">Maximum length of stature.</param>
        /// <returns>Current <see cref="ValidatorBuilder"/> object.</returns>
        public ValidatorBuilder ValidateStature(short minValue, short maxValue)
        {
            this.validators.Add(new StatureValidator(minValue, maxValue));

            return this;
        }

        /// <summary>
        ///     Adds <see cref="WeightValidator"/> into the validation chain.
        /// </summary>
        /// <param name="minValue">Minimal value of weight.</param>
        /// <param name="maxValue">Maximum length of weight.</param>
        /// <returns>Current <see cref="ValidatorBuilder"/> object.</returns>
        public ValidatorBuilder ValidateWeight(decimal minValue, decimal maxValue)
        {
            this.validators.Add(new WeightValidator(minValue, maxValue));

            return this;
        }

        /// <summary>
        ///     Adds <see cref="GenderValidator"/> into the validation chain.
        /// </summary>
        /// <param name="genderValues">Gender's acceptable values.</param>
        /// <returns>Current <see cref="ValidatorBuilder"/> object.</returns>
        public ValidatorBuilder ValidateGender(char[] genderValues)
        {
            this.validators.Add(new GenderValidator(genderValues));

            return this;
        }

        /// <summary>
        ///     Creates <see cref="CompositeValidator"/> object.
        /// </summary>
        /// <returns>Created <see cref="CompositeValidator"/> object with built validation chain.</returns>
        public IRecordValidator Create()
        {
            return new CompositeValidator(this.validators);
        }
    }
}