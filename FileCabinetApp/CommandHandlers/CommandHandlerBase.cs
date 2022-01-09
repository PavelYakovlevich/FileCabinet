using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Abstract class for handlers of program commands.
    /// </summary>
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler? nextHandler;

        /// <inheritdoc cref="ICommandHandler.Handle(AppCommandRequest)"/>
        public virtual void Handle(AppCommandRequest commandRequest)
        {
            this.nextHandler?.Handle(commandRequest);
        }

        /// <inheritdoc cref="ICommandHandler.SetNext(ICommandHandler)"/>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="handler"/> is null.</exception>
        public void SetNext(ICommandHandler handler)
        {
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler), $"{nameof(handler)} can't be null.");
            }

            this.nextHandler = handler;
        }
    }
}
