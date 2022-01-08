using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling of 'remove' command.
    /// </summary>
    public class RemoveCommandHandler : CommandHandlerBase
    {
        /// <summary>
        ///     Handles 'remove' command.
        /// </summary>
        /// <param name="commandRequest">Input data for a requested command.</param>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (!commandRequest.Command.Equals("remove", StringComparison.InvariantCultureIgnoreCase))
            {
                base.Handle(commandRequest);
                return;
            }

            int recordId;
            if (!int.TryParse(commandRequest.Parameters, out recordId))
            {
                Console.WriteLine($"Invalid id '{commandRequest.Parameters}'.");
                return;
            }

            if (recordId < 1)
            {
                Console.WriteLine($"Id can't be lower than 1.");
                return;
            }

            if (!Program.fileCabinetService.RecordExists(recordId))
            {
                Console.WriteLine($"Record #{recordId} doesn't exists.");
                return;
            }

            Program.fileCabinetService.RemoveRecord(recordId);

            Console.WriteLine($"Record #{recordId} is removed.");
        }
    }
}
