using System;
using System.Collections.Generic;

using FileCabinetApp.Printers;
using FileCabinetApp.Services;
using FileCabinetApp.Utils;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling of 'select' command.
    /// </summary>
    public class SelectCommandHandler : ServiceCommandHandlerBase
    {
        private static readonly int FieldSelectionPartIndex = 0;
        private static readonly int SearchConditionPartIndex = 1;

        private static readonly char[] CondtionPartSplitChars = new[] { '=', ' ' };

        private readonly IPrinter<FileCabinetRecord> printer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SelectCommandHandler"/> class.
        /// </summary>
        /// <inheritdoc cref="ServiceCommandHandlerBase.service"/>
        /// <param name="printer"><see cref="IPrinter{T}"/> object, which perfoms printing of objects.</param>
        /// <exception cref="ArgumentNullException">Trown when <paramref name="printer"/> is null.</exception>
        public SelectCommandHandler(IFileCabinetService service, IPrinter<FileCabinetRecord> printer)
            : base(service)
        {
            Guard.ArgumentIsNotNull(printer, nameof(printer));

            this.printer = printer;
        }

        /// <inheritdoc cref="ICommandHandler.Handle(AppCommandRequest)"/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (!commandRequest.Command.Equals("select", StringComparison.InvariantCultureIgnoreCase))
            {
                base.Handle(commandRequest);
                return;
            }

            var splittedParametersParts = commandRequest.Parameters.Split(" where ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (splittedParametersParts.Length == 0)
            {
                Console.WriteLine("Invalid command format.");
                Console.WriteLine("Command must have pattern 'select {selectors} [ where {condtions} ]'.");
                return;
            }

            var fieldSelectionParts = splittedParametersParts[FieldSelectionPartIndex].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (fieldSelectionParts.Length == 0)
            {
                Console.WriteLine("Miising fields selectors.");
                return;
            }

            try
            {
                var printSelectors = ParseSelectors(fieldSelectionParts);

                IEnumerable<FileCabinetRecord> records;
                if (splittedParametersParts.Length > 1)
                {
                    var searchCondtionsParts = splittedParametersParts[SearchConditionPartIndex].Split(CondtionPartSplitChars, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    var searchCondition = SearchInfo<FileCabinetRecord>.Create(RecordsUtils.ConditionCreators, searchCondtionsParts);
                    records = this.service.Find(searchCondition);
                }
                else
                {
                    records = this.service.GetRecords();
                }

                this.printer.Print(records, printSelectors);
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine($"Select error: {exception.Message}");
            }
        }

        private static IDictionary<string, (Func<FileCabinetRecord, string> selector, ValuePadding padding)> ParseSelectors(string[] fieldSelectionParts)
        {
            var result = new Dictionary<string, (Func<FileCabinetRecord, string> selector, ValuePadding padding)>();

            if (fieldSelectionParts.Length == 1 && fieldSelectionParts[0].Equals("*", StringComparison.InvariantCulture))
            {
                return RecordsUtils.FieldsSelectors;
            }

            foreach (var fieldSelector in fieldSelectionParts)
            {
                if (!RecordsUtils.FieldsSelectors.ContainsKey(fieldSelector))
                {
                    throw new ArgumentException($"Unknown '{fieldSelector}' field.");
                }

                result.Add(fieldSelector, RecordsUtils.FieldsSelectors[fieldSelector]);
            }

            return result;
        }
    }
}