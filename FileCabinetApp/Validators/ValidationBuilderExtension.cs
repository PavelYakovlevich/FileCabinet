using System.IO;
using FileCabinetApp.Configuration;
using Microsoft.Extensions.Configuration;

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
        /// <param name="configFilePath">Config file path.</param>
        /// <returns>Built validator.</returns>
        public static IRecordValidator CreateDefault(this ValidatorBuilder builder, string configFilePath)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(configFilePath)
                .Build();

            var validationConfig = config.GetSection("default").Get<ValidationConfig>((options) => options.BindNonPublicProperties = true);

            return BuildValidator(builder, validationConfig);
        }

        /// <summary>
        ///     Creates validator with custom validation rules.
        /// </summary>
        /// <param name="builder">Builder object, which must be appended with custom validators.</param>
        /// <param name="configFilePath">Config file path.</param>
        /// <returns>Built validator.</returns>
        public static IRecordValidator CreateCustom(this ValidatorBuilder builder, string configFilePath)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(configFilePath)
                .Build();

            var validationConfig = config.GetSection("custom").Get<ValidationConfig>((options) => options.BindNonPublicProperties = true);

            return BuildValidator(builder, validationConfig);
        }

        private static IRecordValidator BuildValidator(ValidatorBuilder builder, ValidationConfig validationConfig)
        {
            var validator = builder.ValidateFirstName(validationConfig.FirstName.MinLength, validationConfig.FirstName.MaxLength)
                .ValidateLastName(validationConfig.LastName.MinLength, validationConfig.LastName.MaxLength)
                .ValidateDateOfBirth(validationConfig.DateOfBirth.From, validationConfig.DateOfBirth.To)
                .ValidateStature(validationConfig.Stature.MinValue, validationConfig.Stature.MaxValue)
                .ValidateWeight(validationConfig.Weight.MinValue, validationConfig.Weight.MaxValue)
                .ValidateGender(validationConfig.Gender)
                .Create();

            return validator;
        }
    }
}
