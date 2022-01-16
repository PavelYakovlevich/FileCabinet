using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FileCabinetApp.Services;
using FileCabinetApp.Utils;
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

        /// <summary>
        ///     Initializes a new instance of the <see cref="InsertCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service for working with file cabinet records.</param>
        public InsertCommandHandler(IFileCabinetService service)
            : base(service)
        {
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

            if (fields.Length != RecordsUtils.FieldsSetters.Count)
            {
                Console.WriteLine($"{fields.Length} of {RecordsUtils.FieldsSetters.Count} fields were specified.");
                return;
            }

            if (values.Length != RecordsUtils.FieldsSetters.Count)
            {
                Console.WriteLine($"{values.Length} of {RecordsUtils.FieldsSetters.Count} values were specified.");
                return;
            }

            var record = new FileCabinetRecordParameterObject();

            for (int i = 0; i < fields.Length; i++)
            {
                var fieldName = fields[i].ToLower();

                if (!RecordsUtils.FieldsSetters.ContainsKey(fieldName))
                {
                    Console.WriteLine($"Unknown '{fieldName}' field.");
                    return;
                }

                var fieldValue = values[i].Trim('\'');

                var setResult = RecordsUtils.FieldsSetters[fieldName](record, fieldValue);
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
    }
}
