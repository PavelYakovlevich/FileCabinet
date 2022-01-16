using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Services;
using FileCabinetApp.Validators;

namespace FileCabinetApp.CommandHandlers
{
    public class DeleteCommandHandler : ServiceCommandHandlerBase
    {
        private readonly IDictionary<string, Func<string, Tuple<bool, string, Predicate<FileCabinetRecord>?>>> conditionCreators;
        private readonly IConsoleInputValidator validator;

        public DeleteCommandHandler(IFileCabinetService service, IConsoleInputValidator validator)
            : base(service)
        {
            Guard.ArgumentIsNotNull(validator, nameof(validator));

            this.conditionCreators = new Dictionary<string, Func<string, Tuple<bool, string, Predicate<FileCabinetRecord>?>>>()
            {
                { "id", this.CreateIdCondition },
                { "firstname", this.CreateFirstNameCondition },
                { "lastname", this.CreateLastNameCondition },
                { "dateofbirth", this.CreateDateOFBirthCondition },
                { "stature", this.CreateStatureCondition },
                { "weight", this.CreateWeightCondition },
                { "gender", this.CreateGenderCondition },
            };

            this.validator = validator;
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (!commandRequest.Command.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
            {
                base.Handle(commandRequest);
                return;
            }

            string parameters = commandRequest.Parameters.Trim();

            if (!parameters.StartsWith("where"))
            {
                Console.WriteLine("Missing 'where' keyword.");
                return;
            }

            var searchCriterias = parameters.Substring(5);
            if (string.IsNullOrEmpty(searchCriterias))
            {
                Console.WriteLine("Missing search criteria.");
                return;
            }

            var searchCriteriasPairs = searchCriterias.Split(new[] { '=', ' ' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            try
            {
                var searchCondition = SearchInfo<FileCabinetRecord>.Create(this.conditionCreators, searchCriteriasPairs);

                var deletedRecordsIdsStr = new StringBuilder();
                var deletedRecordsCount = 0;

                var records = this.service.Find(searchCondition);
                foreach (var record in records)
                {
                    this.service.RemoveRecord(record.Id);
                    deletedRecordsIdsStr.Append($" #{record.Id},");
                    deletedRecordsCount++;
                }

                if (deletedRecordsCount != 0)
                {
                    var verbStr = deletedRecordsCount > 1 ? "are" : "is";
                    var recordsString = deletedRecordsCount > 1 ? "Records" : "Record";
                    deletedRecordsIdsStr.Remove(0, 1).Remove(deletedRecordsIdsStr.Length - 1, 1);

                    Console.WriteLine(@$"{recordsString} {deletedRecordsIdsStr} {verbStr} deleted.");
                }
                else
                {
                    Console.WriteLine($"{deletedRecordsCount} records were deleted.");
                }
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine($"Delete error: {exception.Message}");
            }
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

        private Tuple<bool, string, Predicate<FileCabinetRecord>?> CreateIdCondition(string value)
        {
            return CreateCondition(value, InputUtils.IntConverter, this.validator.ValidateId, (record, id) => record.Id == id);
        }

        private Tuple<bool, string, Predicate<FileCabinetRecord>?> CreateFirstNameCondition(string value)
        {
            return CreateCondition(value, InputUtils.StringConverter, this.validator.ValidateFirstName, (record, firstname) => record.FirstName == firstname);
        }

        private Tuple<bool, string, Predicate<FileCabinetRecord>?> CreateLastNameCondition(string value)
        {
            return CreateCondition(value, InputUtils.StringConverter, this.validator.ValidateFirstName, (record, lastname) => record.LastName == lastname);
        }

        private Tuple<bool, string, Predicate<FileCabinetRecord>?> CreateDateOFBirthCondition(string value)
        {
            return CreateCondition(value, InputUtils.DateTimeConverter, this.validator.ValidateBirthDay, (record, dateOfBirth) => record.DateOfBirth == dateOfBirth);
        }

        private Tuple<bool, string, Predicate<FileCabinetRecord>?> CreateStatureCondition(string value)
        {
            return CreateCondition(value, InputUtils.ShortConverter, this.validator.ValidateStature, (record, stature) => record.Stature == stature);
        }

        private Tuple<bool, string, Predicate<FileCabinetRecord>?> CreateWeightCondition(string value)
        {
            return CreateCondition(value, InputUtils.DecimalConverter, this.validator.ValidateWeight, (record, weight) => record.Weight == weight);
        }

        private Tuple<bool, string, Predicate<FileCabinetRecord>?> CreateGenderCondition(string value)
        {
            return CreateCondition(value, InputUtils.CharConverter, this.validator.ValidateGender, (record, gender) => record.Gender == gender);
        }
    }
}