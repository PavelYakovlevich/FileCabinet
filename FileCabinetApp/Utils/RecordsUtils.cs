using System;
using System.Collections.Generic;
using FileCabinetApp.Printers;
using FileCabinetApp.Validators;

#pragma warning disable CS8618

namespace FileCabinetApp.Utils
{
    /// <summary>
    ///     Static class for common data for the finsert, delete, update commands.
    /// </summary>
    public static class RecordsUtils
    {
        private static IConsoleInputValidator validator;

        static RecordsUtils()
        {
            ConditionCreators = new Dictionary<string, Func<string, Tuple<bool, string, Predicate<FileCabinetRecord>?>>>()
            {
                { "id", CreateIdCondition },
                { "firstname", CreateFirstNameCondition },
                { "lastname", CreateLastNameCondition },
                { "dateofbirth", CreateDateOFBirthCondition },
                { "stature", CreateStatureCondition },
                { "weight", CreateWeightCondition },
                { "gender", CreateGenderCondition },
            };

            FieldsSetters = new Dictionary<string, Func<FileCabinetRecordParameterObject, string, Tuple<bool, string>>>()
            {
                { "id", SetId },
                { "firstname", SetFirstName },
                { "lastname", SetLastName },
                { "dateofbirth", SetDateOfBirth },
                { "stature", SetStature },
                { "weight", SetWeight },
                { "gender", SetGender },
            };

            FieldsSelectors = new Dictionary<string, (Func<FileCabinetRecord, string> selector, ValuePadding padding)>()
            {
                { "id", (record => record.Id.ToString(), ValuePadding.Left) },
                { "firstname", (record => record.FirstName, ValuePadding.Right) },
                { "lastname", (record => record.LastName, ValuePadding.Right) },
                { "dateofbirth", (record => record.DateOfBirth.ToString("MM/dd/yyyy"), ValuePadding.Left) },
                { "stature", (record => record.Stature.ToString(), ValuePadding.Left) },
                { "weight", (record => record.Weight.ToString(), ValuePadding.Left) },
                { "gender", (record => record.Gender.ToString(), ValuePadding.Left) },
            };
        }

        /// <summary>
        ///     Gets condition creators dictionary.
        /// </summary>
        /// <value>Condition creators dictionary.</value>
        public static Dictionary<string, Func<string, Tuple<bool, string, Predicate<FileCabinetRecord>?>>> ConditionCreators { get; }

        /// <summary>
        ///     Gets fields setters dictionary.
        /// </summary>
        /// <value>Fields setters dictionary.</value>
        public static Dictionary<string, Func<FileCabinetRecordParameterObject, string, Tuple<bool, string>>> FieldsSetters { get; }

        /// <summary>
        ///     Gets fields selectors dictionary.
        /// </summary>
        /// <value>Fields selectors dictionary.</value>
        public static Dictionary<string, (Func<FileCabinetRecord, string> selector, ValuePadding padding)> FieldsSelectors { get; }

        /// <summary>
        ///     Initializes class data.
        /// </summary>
        /// <param name="inputValidator">Validator, which will validates input for the proper fields.</param>
        public static void Initialize(IConsoleInputValidator inputValidator)
        {
            Guard.ArgumentIsNotNull(inputValidator, nameof(inputValidator));

            validator = inputValidator;
        }

        private static Tuple<bool, string, Predicate<FileCabinetRecord>?> CreateCondition<T>(string value, Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator, Func<FileCabinetRecord, T, bool> condition)
        {
            var conversionResult = converter(value);
            if (!conversionResult.Item1)
            {
                return new Tuple<bool, string, Predicate<FileCabinetRecord>?>(false, $"conversion failed with an error '{conversionResult.Item2}'", null);
            }

            var validationResult = validator(conversionResult.Item3);
            if (!validationResult.Item1)
            {
                return new Tuple<bool, string, Predicate<FileCabinetRecord>?>(false, $"validation failed with an error '{validationResult.Item2}'", null);
            }

            return new Tuple<bool, string, Predicate<FileCabinetRecord>?>(true, string.Empty, (record) => condition(record, conversionResult.Item3));
        }

        private static Tuple<bool, string> SetFieldValue<T>(FileCabinetRecordParameterObject record, string value, Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator, Action<FileCabinetRecordParameterObject, T> setter)
        {
            var conversionResult = converter(value);
            if (!conversionResult.Item1)
            {
                return new Tuple<bool, string>(false, $"conversion failed with an error '{conversionResult.Item2}'");
            }

            var validationResult = validator(conversionResult.Item3);
            if (!validationResult.Item1)
            {
                return new Tuple<bool, string>(false, $"validation failed with an error '{validationResult.Item2}'");
            }

            setter(record, conversionResult.Item3);

            return new Tuple<bool, string>(true, string.Empty);
        }

        private static Tuple<bool, string, Predicate<FileCabinetRecord>?> CreateIdCondition(string value)
        {
            return CreateCondition(value, InputUtils.IntConverter, validator.ValidateId, (record, id) => record.Id == id);
        }

        private static Tuple<bool, string, Predicate<FileCabinetRecord>?> CreateFirstNameCondition(string value)
        {
            return CreateCondition(value, InputUtils.StringConverter, validator.ValidateFirstName, (record, firstname) => record.FirstName == firstname);
        }

        private static Tuple<bool, string, Predicate<FileCabinetRecord>?> CreateLastNameCondition(string value)
        {
            return CreateCondition(value, InputUtils.StringConverter, validator.ValidateFirstName, (record, lastname) => record.LastName == lastname);
        }

        private static Tuple<bool, string, Predicate<FileCabinetRecord>?> CreateDateOFBirthCondition(string value)
        {
            return CreateCondition(value, InputUtils.DateTimeConverter, validator.ValidateBirthDay, (record, dateOfBirth) => record.DateOfBirth == dateOfBirth);
        }

        private static Tuple<bool, string, Predicate<FileCabinetRecord>?> CreateStatureCondition(string value)
        {
            return CreateCondition(value, InputUtils.ShortConverter, validator.ValidateStature, (record, stature) => record.Stature == stature);
        }

        private static Tuple<bool, string, Predicate<FileCabinetRecord>?> CreateWeightCondition(string value)
        {
            return CreateCondition(value, InputUtils.DecimalConverter, validator.ValidateWeight, (record, weight) => record.Weight == weight);
        }

        private static Tuple<bool, string, Predicate<FileCabinetRecord>?> CreateGenderCondition(string value)
        {
            return CreateCondition(value, InputUtils.CharConverter, validator.ValidateGender, (record, gender) => record.Gender == gender);
        }

        private static Tuple<bool, string> SetId(FileCabinetRecordParameterObject record, string value)
        {
            return SetFieldValue(record, value, InputUtils.IntConverter, validator.ValidateId, (record, id) => record.Id = id);
        }

        private static Tuple<bool, string> SetFirstName(FileCabinetRecordParameterObject record, string value)
        {
            return SetFieldValue(record, value, InputUtils.StringConverter, validator.ValidateFirstName, (record, firstname) => record.FirstName = firstname);
        }

        private static Tuple<bool, string> SetLastName(FileCabinetRecordParameterObject record, string value)
        {
            return SetFieldValue(record, value, InputUtils.StringConverter, validator.ValidateLastName, (record, lastname) => record.LastName = lastname);
        }

        private static Tuple<bool, string> SetDateOfBirth(FileCabinetRecordParameterObject record, string value)
        {
            return SetFieldValue(record, value, InputUtils.DateTimeConverter, validator.ValidateBirthDay, (record, dateTime) => record.DateOfBirth = dateTime);
        }

        private static Tuple<bool, string> SetStature(FileCabinetRecordParameterObject record, string value)
        {
            return SetFieldValue(record, value, InputUtils.ShortConverter, validator.ValidateStature, (record, stature) => record.Stature = stature);
        }

        private static Tuple<bool, string> SetWeight(FileCabinetRecordParameterObject record, string value)
        {
            return SetFieldValue(record, value, InputUtils.DecimalConverter, validator.ValidateWeight, (record, weight) => record.Weight = weight);
        }

        private static Tuple<bool, string> SetGender(FileCabinetRecordParameterObject record, string value)
        {
            return SetFieldValue(record, value, InputUtils.CharConverter, validator.ValidateGender, (record, gender) => record.Gender = gender);
        }
    }
}