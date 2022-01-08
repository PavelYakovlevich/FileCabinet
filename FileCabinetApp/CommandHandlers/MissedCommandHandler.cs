using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling unknown command.
    /// </summary>
    public class MissedCommandHandler : CommandHandlerBase
    {
        /// <summary>
        ///     Handles unknown command request.
        /// </summary>
        /// <param name="commandRequest">Input data for a requested command.</param>
        public override void Handle(AppCommandRequest commandRequest)
        {
            Console.WriteLine($"There is no '{commandRequest.Command}' command.");
            Console.WriteLine();
        }
    }
}
