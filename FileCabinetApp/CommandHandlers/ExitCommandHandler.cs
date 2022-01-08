using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling of 'exit' command.
    /// </summary>
    public class ExitCommandHandler : CommandHandlerBase
    {
        private Action<bool> onExitAction;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExitCommandHandler"/> class.
        /// </summary>
        /// <param name="action">Action, which is executed after exit command was entered.</param>
        public ExitCommandHandler(Action<bool> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action), $"{nameof(action)} can't be null.");
            }

            this.onExitAction = action;
        }

        /// <summary>
        ///     Handles 'exit' command.
        /// </summary>
        /// <param name="commandRequest">Input data for a requested command.</param>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (!commandRequest.Command.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
            {
                base.Handle(commandRequest);
                return;
            }

            Console.WriteLine("Exiting an application...");

            this.onExitAction(false);
        }
    }
}
