using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FileCabinetApp.Services;
using FileCabinetApp.Validators;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling of 'insert' command.
    /// </summary>
    public class InsertCommandHandler : ServiceCommandHandlerBase
    {
        private static readonly int FieldsNamesIndex = 0;
        private static readonly int FieldsValuesIndex = 1;
        private static readonly char[] ValuesSeparators = new[] { '(', ',', ')' };
        private static readonly Regex FieldsPartRegex = new Regex(@"^\([ ]*\w+([ ]*,[ ]*\w+)*[ ]*\)$");
        private static readonly Regex ValuesPartRegex = new Regex(@"^\([ ]*\'[\w'\\\/+\- ]*\'([ ]*,[ ]*\'[\w'\\\/+\- ]*\')*[ ]*\)$");

        private readonly IDictionary<string, Func<FileCabinetRecordParameterObject, string, Tuple<bool, string>>> fieldsSetters;
        private readonly IConsoleInputValidator validator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InsertCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service for working with file cabinet records.</param>
        /// <param name="validator">Validator for console input.</param>
        public InsertCommandHandler(IFileCabinetService service, IConsoleInputValidator validator)
            : base(service)
        {
            Guard.ArgumentIsNotNull(validator, nameof(validator));

            this.validator = validator;
            this.fieldsSetters = new Dictionary<string, Func<FileCabinetRecordParameterObject, string, Tuple<bool, string>>>()
            {
                { "id", this.SetId },
                { "firstname", this.SetFirstName },
                { "lastname", this.SetLastName },
                { "dateofbirth", this.SetDateOfBirth },
                { "stature", this.SetStature },
                { "weight", this.SetWeight },
                { "gender", this.SetGender },
            };
        }

        /// <inheritdoc cref="ICommandHandler.Handle(AppCommandRequest)"/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (!commandRequest.Command.Equals("insert", StringComparison.InvariantCultureIgnoreCase))
            {
                base.Handle(commandRequest);
                return;
            }

            var parametersParts = commandRequest.Parameters.Split(" values ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parametersParts.Length < 2)
            {
                Console.WriteLine("Command must be insert (field1,...) values ('value',...).");
                return;
            }

            if (!FieldsPartRegex.IsMatch(parametersParts[FieldsNamesIndex]))
            {
                Console.WriteLine("Invalid format of fields part.");
                return;
            }

            var fields = parametersParts[FieldsNamesIndex].Split(ValuesSeparators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (!ValuesPartRegex.IsMatch(parametersParts[FieldsValuesIndex]))
            {
                Console.WriteLine("Invalid format of fields values part.");
                return;
            }

            var values = parametersParts[FieldsValuesIndex].Split(ValuesSeparators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (fields.Length != this.fieldsSetters.Count)
            {
                Console.WriteLine($"{fields.Length} of {this.fieldsSetters.Count} fields were specified.");
                return;
            }

            if (values.Length != this.fieldsSetters.Count)
            {
                Console.WriteLine($"{values.Length} of {this.fieldsSetters.Count} values were specified.");
                return;
            }

            var record = new FileCabinetRecordParameterObject();

            for (int i = 0; i < fields.Length; i++)
            {
                var fieldName = fields[i].ToLower();

                if (!this.fieldsSetters.ContainsKey(fieldName))
                {
                    Console.WriteLine($"Unknown '{fieldName}' field.");
                    return;
                }

                var fieldValue = values[i].Trim('\'');

                var setResult = this.fieldsSetters[fieldName](record, fieldValue);
                if (!setResult.Item1)
                {
                    Console.WriteLine($"Set of '{fieldName}' field error: {setResult.Item2}.");
                    return;
                }
            }

            if (this.service.RecordExists(record.Id))
            {
                Console.WriteLine($"Record with '{record.Id}' id is already exist.");
                return;
            }

            this.service.CreateRecord(record);
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

        private Tuple<bool, string> SetId(FileCabinetRecordParameterObject record, string value)
        {
            return SetFieldValue(record, value, InputUtils.IntConverter, this.validator.ValidateId, (record, id) => record.Id = id);
        }

        private Tuple<bool, string> SetFirstName(FileCabinetRecordParameterObject record, string value)
        {
            return SetFieldValue(record, value, InputUtils.StringConverter, this.validator.ValidateFirstName, (record, firstname) => record.FirstName = firstname);
        }

        private Tuple<bool, string> SetLastName(FileCabinetRecordParameterObject record, string value)
        {
            return SetFieldValue(record, value, InputUtils.StringConverter, this.validator.ValidateLastName, (record, lastname) => record.LastName = lastname);
        }

        private Tuple<bool, string> SetDateOfBirth(FileCabinetRecordParameterObject record, string value)
        {
            return SetFieldValue(record, value, InputUtils.DateTimeConverter, this.validator.ValidateBirthDay, (record, dateTime) => record.DateOfBirth = dateTime);
        }

        private Tuple<bool, string> SetStature(FileCabinetRecordParameterObject record, string value)
        {
            return SetFieldValue(record, value, InputUtils.ShortConverter, this.validator.ValidateStature, (record, stature) => record.Stature = stature);
        }

        private Tuple<bool, string> SetWeight(FileCabinetRecordParameterObject record, string value)
        {
            return SetFieldValue(record, value, InputUtils.DecimalConverter, this.validator.ValidateWeight, (record, weight) => record.Weight = weight);
        }

        private Tuple<bool, string> SetGender(FileCabinetRecordParameterObject record, string value)
        {
            return SetFieldValue(record, value, InputUtils.CharConverter, this.validator.ValidateGender, (record, gender) => record.Gender = gender);
        }
    }
}
