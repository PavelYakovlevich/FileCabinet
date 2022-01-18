using FileCabinetApp.Services;

#pragma warning disable SA1401

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handlers of program commands, which require <see cref="IFileCabinetService"/> object.
    /// </summary>
    public abstract class ServiceCommandHandlerBase : CommandHandlerBase
    {
        /// <summary>
        ///     Service for working with file cabinet records.
        /// </summary>
        protected readonly IFileCabinetService service;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ServiceCommandHandlerBase"/> class.
        /// </summary>
        /// <param name="service">Service for working with file cabinet records.</param>
        protected ServiceCommandHandlerBase(IFileCabinetService service)
        {
            this.service = service;
        }
    }
}