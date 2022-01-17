using System;
using System.Text;

using FileCabinetApp.Services;
using FileCabinetApp.Utils;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling 'delete' command.
    /// </summary>
    public class DeleteCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DeleteCommandHandler"/> class.
        /// </summary>
        /// <inheritdoc cref="ServiceCommandHandlerBase"/>
        public DeleteCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        ///     Handles delete command.
        /// </summary>
        /// <inheritdoc cref="ICommandHandler.Handle(AppCommandRequest)"/>
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
                var searchCondition = SearchInfo<FileCabinetRecord>.Create(RecordsUtils.ConditionCreators, searchCriteriasPairs);

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
    }
}