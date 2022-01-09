namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Class with extension methods for <see cref="ValidatorBuilder"/>.
    /// </summary>
    public static class ValidationBuilderExtension
    {
        /// <summary>
        ///     Creates validator with default validation rules.
        /// </summary>
        /// <param name="builder">Builder object, which must be appended with default validators.</param>
        /// <returns>Built validator.</returns>
        public static IRecordValidator CreateDefault(this ValidatorBuilder builder)
        {
            var validator = builder.ValidateFirstName(ValidationConstraints.DefaultFirstNameMinLength, ValidationConstraints.DefaultFirstNameMaxLength)
                    .ValidateLastName(ValidationConstraints.DefaultLastNameMinLength, ValidationConstraints.DefaultLastNameMaxLength)
                    .ValidateDateOfBirth(ValidationConstraints.DefaultDateOfBirthMinValue, ValidationConstraints.DefaultDateOfBirthMaxValue)
                    .ValidateStature(ValidationConstraints.DefaultStatureMinValue, ValidationConstraints.DefaultStatureMaxValue)
                    .ValidateWeight(ValidationConstraints.DefaultWeightMinValue, ValidationConstraints.DefaultWeightMaxValue)
                    .ValidateGender(ValidationConstraints.DefaultGenderValues)
                    .Create();

            return validator;
        }

        /// <summary>
        ///     Creates validator with custom validation rules.
        /// </summary>
        /// <param name="builder">Builder object, which must be appended with custom validators.</param>
        /// <returns>Built validator.</returns>
        public static IRecordValidator CreateCustom(this ValidatorBuilder builder)
        {
            var validator = builder.ValidateFirstName(ValidationConstraints.CustomFirstNameMinLength, ValidationConstraints.CustomFirstNameMaxLength)
                    .ValidateLastName(ValidationConstraints.CustomLastNameMinLength, ValidationConstraints.CustomLastNameMaxLength)
                    .ValidateDateOfBirth(ValidationConstraints.CustomDateOfBirthMinValue, ValidationConstraints.CustomDateOfBirthMaxValue)
                    .ValidateStature(ValidationConstraints.CustomStatureMinValue, ValidationConstraints.CustomStatureMaxValue)
                    .ValidateWeight(ValidationConstraints.CustomWeightMinValue, ValidationConstraints.CustomWeightMaxValue)
                    .ValidateGender(ValidationConstraints.CustomGenderValues)
                    .Create();

            return validator;
        }
    }
}
