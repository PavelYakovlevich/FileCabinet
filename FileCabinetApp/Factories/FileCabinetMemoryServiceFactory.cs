using FileCabinetApp.Services;

namespace FileCabinetApp.Factories
{
    public class FileCabinetMemoryServiceFactory : IFileCabinetServiceFactory
    {
        public IFileCabinetService Create(IRecordValidator validator)
        {
            return new FileCabinetMemoryService(validator);
        }
    }
}
