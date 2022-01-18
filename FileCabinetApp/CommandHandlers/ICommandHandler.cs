namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Interface for handlers of program commands.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        ///     Sets next command handler, which will be called if current handler can't handle a program command request.
        /// </summary>
        /// <param name="handler">Next handler, which will be called if current handler can't handle a program command request.</param>
        /// <returns><paramref name="handler"/> object.</returns>
        ICommandHandler SetNext(ICommandHandler handler);

        /// <summary>
        ///     Handles command request.
        /// </summary>
        /// <param name="commandRequest">Input data for a requested command.</param>
        void Handle(AppCommandRequest commandRequest);
    }
}
