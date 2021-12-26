using FileCabinetApp.Validators;

namespace FileCabinetApp.Services
{
    /// <summary>
    ///     Class for the custom implementaion of <see cref="FileCabinetService"/>.
    /// </summary>
    internal class FileCabinetCustomService : FileCabinetService
    {
        protected override IRecordValidator CreateValidator()
        {
            return new CustomFileRecordValidator();
        }
    }
}
