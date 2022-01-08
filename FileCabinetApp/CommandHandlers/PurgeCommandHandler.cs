using System;

using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling of 'purge' command.
    /// </summary>
    public class PurgeCommandHandler : CommandHandlerBase
    {
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

            if (Program.fileCabinetService is not FileCabinetFilesystemService)
            {
                Console.WriteLine("This command is only allowed for the filesystem service.");
                return;
            }

            var fileSystemService = (FileCabinetFilesystemService)Program.fileCabinetService;

            var recordsAmount = fileSystemService.GetStat().total;

            fileSystemService.Purge();

            var purgedRecords = recordsAmount - Program.fileCabinetService.GetStat().total;

            Console.WriteLine($"Data file processing is completed: {purgedRecords} of {recordsAmount} records were purged.");
        }
    }
}
