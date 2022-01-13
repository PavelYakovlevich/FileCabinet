using System;
using System.Collections.ObjectModel;
using FileCabinetApp.Printers;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling of 'find' command.
    /// </summary>
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private IRecordPrinter printer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service for working with file cabinet records.</param>
        /// <param name="printer">Object, which will be responsible for printing records.</param>
        public FindCommandHandler(IFileCabinetService service, IRecordPrinter printer)
            : base(service)
        {
            if (printer is null)
            {
                throw new ArgumentNullException(nameof(printer), $"{nameof(printer)} can't be null.");
            }

            this.printer = printer;
        }

        /// <summary>
        ///     Handles 'find' command.
        /// </summary>
        /// <param name="commandRequest">Input data for a requested command.</param>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (!commandRequest.Command.Equals("find", StringComparison.InvariantCultureIgnoreCase))
            {
                base.Handle(commandRequest);
                return;
            }

            var values = commandRequest.Parameters.Split(' ', 2);

            if (values.Length < 2)
            {
                Console.WriteLine("Parameter name or parameter value is missing!");
                return;
            }

            var paramName = values[0];
            var paramValue = values[1].Trim();

            if (paramValue.Length < 3)
            {
                Console.WriteLine("Parameter value is missing!");
                return;
            }

            paramValue = values[1].Substring(1, values[1].Length - 2);

            IRecordIterator iterator;
            if (paramName.Equals("firstname", StringComparison.InvariantCultureIgnoreCase))
            {
                iterator = this.service.FindByFirstName(paramValue);
            }
            else if (paramName.Equals("lastname", StringComparison.InvariantCultureIgnoreCase))
            {
                iterator = this.service.FindByLastName(paramValue);
            }
            else if (paramName.Equals("dateofbirth", StringComparison.InvariantCultureIgnoreCase))
            {
                DateTime dateOfBirth;
                if (!DateTime.TryParse(paramValue, out dateOfBirth))
                {
                    Console.WriteLine("Invalid format of date!");
                    return;
                }

                iterator = this.service.FindByDateOfBirth(dateOfBirth);
            }
            else
            {
                Console.WriteLine("Search criteria by specified parameter is not defined!");
                return;
            }

            while (iterator.HasMore())
            {
                var record = iterator.GetNext();
                this.printer.Print(record);
            }
        }
    }
}
