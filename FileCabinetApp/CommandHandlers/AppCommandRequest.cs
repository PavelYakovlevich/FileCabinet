namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for program command's data.
    /// </summary>
    public class AppCommandRequest
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AppCommandRequest"/> class.
        /// </summary>
        /// <param name="command">Command's name.</param>
        /// <param name="parameters">Command's parameters string.</param>
        public AppCommandRequest(string command, string parameters)
        {
            this.Command = command;
            this.Parameters = parameters;
        }

        /// <summary>
        ///     Gets command's name.
        /// </summary>
        /// <value>Command's name.</value>
        public string Command { get; }

        /// <summary>
        ///     Gets command's parameters string.
        /// </summary>
        /// <value>Command's parameters string.</value>
        public string Parameters { get; }
    }
}