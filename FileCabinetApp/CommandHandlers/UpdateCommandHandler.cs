using System;
using FileCabinetApp.Services;
using FileCabinetApp.Utils;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling of 'update' command.
    /// </summary>
    public class UpdateCommandHandler : ServiceCommandHandlerBase
    {
        private static readonly int FieldsNamesIndex = 0;
        private static readonly int FieldsValuesIndex = 1;
        private static readonly char[] SplitChars = new[] { '=', ' ' };

        /// <summary>
        ///     Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service for working with file cabinet records.</param>
        public UpdateCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        ///     Handles update command.
        /// </summary>
        /// <inheritdoc cref="ICommandHandler.Handle(AppCommandRequest)"/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (!commandRequest.Command.Equals("update", StringComparison.InvariantCultureIgnoreCase))
            {
                base.Handle(commandRequest);
                return;
            }

            var divideBySetParts = commandRequest.Parameters.Split("set", StringSplitOptions.TrimEntries);
            if (divideBySetParts.Length < 2)
            {
                Console.WriteLine("Missing 'set' keyword.");
                return;
            }

            if (divideBySetParts.Length > 2)
            {
                Console.WriteLine("'set' keyword must be presented only once.");
                return;
            }

            var commandParts = divideBySetParts[1].Split(" where ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (commandParts.Length < 2)
            {
                Console.WriteLine("Missing 'where' keyword or field setters or conditions part.");
                return;
            }

            if (commandParts.Length > 2)
            {
                Console.WriteLine("'where' keyword must be presented only once.");
                return;
            }

            var setValuesParts = commandParts[0];
            var whereStatementPart = commandParts[1].Split(SplitChars, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            try
            {
                var updatedRecordParams = this.ParseRecordParamsObject(setValuesParts);

                var searchCondition = SearchInfo<FileCabinetRecord>.Create(RecordsUtils.ConditionCreators, whereStatementPart);

                var records = this.service.Find(searchCondition);

                foreach (var record in records)
                {
                    var paramObject = this.FillParamObject(updatedRecordParams, record);

                    this.service.EditRecord(paramObject);
                }
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine($"Update error: {exception.Message}");
            }
        }

        private FileCabinetRecordParameterObject ParseRecordParamsObject(string setValuesString)
        {
            var result = new FileCabinetRecordParameterObject();

            var setValuesPairs = setValuesString.Split(',', StringSplitOptions.TrimEntries);
            foreach (var setValuesPair in setValuesPairs)
            {
                if (string.IsNullOrEmpty(setValuesPair))
                {
                    throw new ArgumentException("Comma must be present only before preceding field setter statement.");
                }

                var setValuesSplittedPair = setValuesPair.Split('=', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (setValuesSplittedPair.Length != 2)
                {
                    throw new ArgumentException($"Missing value for the {setValuesPair[0]}.");
                }

                var fieldName = setValuesSplittedPair[FieldsNamesIndex].ToLower();
                if (!RecordsUtils.FieldsSetters.ContainsKey(fieldName))
                {
                    throw new ArgumentException($"Unknown field '{fieldName}'");
                }

                if (fieldName.Equals("id", StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new ArgumentException($"Setting of field '{fieldName}' is forbidden.");
                }

                var fieldValue = setValuesSplittedPair[FieldsValuesIndex].Trim('\'');

                var setResult = RecordsUtils.FieldsSetters[fieldName](result, fieldValue);
                if (!setResult.Item1)
                {
                    throw new ArgumentException($"Set of '{fieldName}' field error: {setResult.Item2}.");
                }
            }

            return result;
        }

        private FileCabinetRecordParameterObject FillParamObject(FileCabinetRecordParameterObject paramObject, FileCabinetRecord record)
        {
            var result = (FileCabinetRecordParameterObject)paramObject.Clone();

            result.Id = record.Id;

            if (paramObject.FirstName is null)
            {
                result.FirstName = record.FirstName;
            }

            if (paramObject.LastName is null)
            {
                result.LastName = record.LastName;
            }

            if (paramObject.DateOfBirth.Equals(default))
            {
                result.DateOfBirth = record.DateOfBirth;
            }

            if (paramObject.Gender.Equals(default))
            {
                result.Gender = record.Gender;
            }

            if (paramObject.Weight.Equals(default))
            {
                result.Weight = record.Weight;
            }

            if (paramObject.Stature.Equals(default))
            {
                result.Stature = record.Stature;
            }

            return result;
        }
    }
}