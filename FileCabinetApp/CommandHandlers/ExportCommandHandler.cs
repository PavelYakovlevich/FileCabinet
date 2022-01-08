﻿using System;
using System.IO;

using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling of 'export' command.
    /// </summary>
    public class ExportCommandHandler : CommandHandlerBase
    {
        private readonly IFileCabinetService service;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExportCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service for working with file cabinet records.</param>
        public ExportCommandHandler(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <summary>
        ///     Handles 'export' command.
        /// </summary>
        /// <param name="commandRequest">Input data for a requested command.</param>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (!commandRequest.Command.Equals("export", StringComparison.InvariantCultureIgnoreCase))
            {
                base.Handle(commandRequest);
                return;
            }

            if (this.service is FileCabinetFilesystemService)
            {
                Console.WriteLine("This command is not allowed with --storage=file");
                return;
            }

            var parametersValues = commandRequest.Parameters.Split(' ');

            if (parametersValues.Length < 2)
            {
                Console.WriteLine("Missing export method or file path.");
                return;
            }

            var exportMethod = parametersValues[0];
            if (!exportMethod.Equals("csv", StringComparison.InvariantCultureIgnoreCase) && !exportMethod.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine($"Unsupported export method: {exportMethod}");
                return;
            }

            var snapshotFilePath = parametersValues[1];
            if (File.Exists(snapshotFilePath))
            {
                Console.Write($"File is exist - rewrite {snapshotFilePath}? [Y/n] ");
                var userAnswer = Console.ReadKey().KeyChar;
                Console.WriteLine();

                userAnswer = char.ToUpperInvariant(userAnswer);
                if (userAnswer != 'Y')
                {
                    return;
                }
            }

            try
            {
                using (var writer = new StreamWriter(snapshotFilePath))
                {
                    var snapshot = ((FileCabinetMemoryService)this.service).MakeSnapshot();

                    if (exportMethod.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
                    {
                        snapshot.SaveToCsv(writer);
                    }
                    else if (exportMethod.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
                    {
                        snapshot.SaveToXml(writer);
                    }

                    writer.Close();
                }
            }
            catch (IOException exception)
            {
                Console.WriteLine($"Export failed: {exception.Message}");
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                Console.WriteLine($"Access error: {unauthorizedAccessException.Message}");
            }
        }
    }
}