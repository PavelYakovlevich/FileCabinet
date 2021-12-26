using FileCabinetApp.Validators;

namespace FileCabinetApp.Services
{
    /// <summary>
    ///     Class for the custom implementaion of <see cref="FileCabinetService"/>.
    /// </summary>
    internal class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FileCabinetCustomService"/> class.
        /// </summary>
        public FileCabinetCustomService()
            : base(new CustomFileRecordValidator())
        {
        }
    }
}
