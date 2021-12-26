using FileCabinetApp.Validators;

namespace FileCabinetApp.Services
{
    /// <summary>
    ///     Class for the default implementation of the <see cref="FileCabinetService"/>.
    /// </summary>
    internal class FileCabinetDefaultService : FileCabinetService
    {
        protected override IRecordValidator CreateValidator()
        {
            return new DefaultFileRecordValidator();
        }
    }
}
