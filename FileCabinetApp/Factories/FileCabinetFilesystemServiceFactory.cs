using FileCabinetApp.Services;

namespace FileCabinetApp.Factories
{
    public class FileCabinetFilesystemServiceFactory : IFileCabinetServiceFactory
    {
        public IFileCabinetService Create(IRecordValidator validator)
        {
            return new FileCabinetFilesystemService(validator);
        }
    }
}
