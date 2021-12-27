using FileCabinetApp.Services;

namespace FileCabinetApp.Factories
{
    public interface IFileCabinetServiceFactory
    {
        IFileCabinetService Create(IRecordValidator validator);
    }
}
