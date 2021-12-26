using FileCabinetApp.Validators;

namespace FileCabinetApp.Services
{
    /// <summary>
    ///     Class for the default implementation of the <see cref="FileCabinetService"/>.
    /// </summary>
    internal class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FileCabinetDefaultService"/> class.
        /// </summary>
        public FileCabinetDefaultService()
            : base(new DefaultFileRecordValidator())
        {
        }
    }
}
