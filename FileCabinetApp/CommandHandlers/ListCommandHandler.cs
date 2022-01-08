using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling of 'list' command.
    /// </summary>
    public class ListCommandHandler : CommandHandlerBase
    {
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

            var records = Program.fileCabinetService.GetRecords();

            foreach (var record in records)
            {
                Console.WriteLine($"#{record}");
            }
        }
    }
}
