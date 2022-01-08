using System;

using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling of 'list' command.
    /// </summary>
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ListCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service for working with file cabinet records.</param>
        public ListCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        ///     Handles 'list' command.
        /// </summary>
        /// <param name="commandRequest">Input data for a requested command.</param>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (!commandRequest.Command.Equals("list", StringComparison.InvariantCultureIgnoreCase))
            {
                base.Handle(commandRequest);
                return;
            }

            var records = this.service.GetRecords();

            foreach (var record in records)
            {
                Console.WriteLine($"#{record}");
            }
        }
    }
}
