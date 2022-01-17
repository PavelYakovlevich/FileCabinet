using System;
using System.Collections.Generic;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling unknown command.
    /// </summary>
    public class MissedCommandHandler : CommandHandlerBase
    {
        private static readonly string[] DefineCommands = new string[]
        {
            "help",
            "exit",
            "stat",
            "create",
            "list",
            "find",
            "export",
            "import",
            "purge",
            "insert",
            "delete",
            "update",
        };

        /// <summary>
        ///     Handles unknown command request.
        /// </summary>
        /// <param name="commandRequest">Input data for a requested command.</param>
        public override void Handle(AppCommandRequest commandRequest)
        {
            Console.WriteLine($"There is no '{commandRequest.Command}' command.");
            Console.WriteLine();

            var similarCommands = new List<string>();

            var command = commandRequest.Command;
            foreach (var definedCommand in DefineCommands)
            {
                if (AreSimilarCommands(definedCommand, command))
                {
                    similarCommands.Add(definedCommand);
                }
            }

            if (similarCommands.Count == 0)
            {
                return;
            }

            if (similarCommands.Count == 1)
            {
                Console.WriteLine("The most similar command is");
            }
            else
            {
                Console.WriteLine("The most similar commands are");
            }

            foreach (var similarCommand in similarCommands)
            {
                Console.WriteLine($"\t{similarCommand}");
            }
        }

        private static bool AreSimilarCommands(string definedCommand, string command)
        {
            if (definedCommand.StartsWith(command) || command.StartsWith(definedCommand))
            {
                return true;
            }

            for (int i = command.Length - 1; i > 0; i--)
            {
                if (definedCommand.StartsWith(command.Substring(0, i)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
