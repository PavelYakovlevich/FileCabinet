using System;

using FileCabinetApp.Services;
using FileCabinetApp.Utils;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling of 'purge' command.
    /// </summary>
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service for working with file cabinet records.</param>
        public PurgeCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

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

            var wrappedObject = (this.service as ServiceWrapperBase)?.GetWrappedObject();
            if (wrappedObject is not FileCabinetFilesystemService)
            {
                Console.WriteLine("Usage of this command is allowed only for the FilesystemService");
                return;
            }

            var recordsAmount = wrappedObject.GetStat().total;

            this.service.Purge();

            var purgedRecords = recordsAmount - wrappedObject.GetStat().total;

            Console.WriteLine($"Data file processing is completed: {purgedRecords} of {recordsAmount} records were purged.");
        }
    }
}
