using System;

using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling of 'purge' command.
    /// </summary>
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service for working with file cabinet records.</param>
        public PurgeCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        ///     Handles 'purge' command.
        /// </summary>
        /// <param name="commandRequest">Input data for a requested command.</param>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (!commandRequest.Command.Equals("purge", StringComparison.InvariantCultureIgnoreCase))
            {
                base.Handle(commandRequest);
                return;
            }

            if (this.service is not FileCabinetFilesystemService)
            {
                Console.WriteLine("This command is only allowed for the filesystem service.");
                return;
            }

            var fileSystemService = (FileCabinetFilesystemService)this.service;

            var recordsAmount = fileSystemService.GetStat().total;

            fileSystemService.Purge();

            var purgedRecords = recordsAmount - this.service.GetStat().total;

            Console.WriteLine($"Data file processing is completed: {purgedRecords} of {recordsAmount} records were purged.");
        }
    }
}
