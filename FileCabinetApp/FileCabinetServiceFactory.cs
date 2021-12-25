using System;

using FileCabinetApp.Services;

namespace FileCabinetApp
{
    internal static class FileCabinetServiceFactory
    {
        internal static FileCabinetService Create(string name)
        {
            return name switch
            {
                "custom" => new FileCabinetCustomService(),
                _ => new FileCabinetDefaultService()
            };
        }
    }
}
