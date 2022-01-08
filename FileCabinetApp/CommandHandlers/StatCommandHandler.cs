using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling of 'stat' command.
    /// </summary>
    public class StatCommandHandler : CommandHandlerBase
    {
        /// <summary>
        ///     Handles 'stat' command.
        /// </summary>
        /// <param name="commandRequest">Input data for a requested command.</param>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (!commandRequest.Command.Equals("stat", StringComparison.InvariantCultureIgnoreCase))
            {
                base.Handle(commandRequest);
                return;
            }

            var recordsInfo = Program.fileCabinetService.GetStat();
            Console.WriteLine($"Total amount of records: {recordsInfo.total}.");
            Console.WriteLine($"Delete records amount: {recordsInfo.deleted}.");
        }
    }
}
