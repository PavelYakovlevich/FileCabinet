using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling of 'help' command.
    /// </summary>
    public class HelpCommandHandler : CommandHandlerBase
    {
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static string[][] helpMessages = new string[][]
        {
                    new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
                    new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
                    new string[] { "stat", "prints count of registered users", "The 'stat' command prints count of registered users." },
                    new string[] { "create", "creates a new user", "The 'create' command creates a new user." },
                    new string[] { "list", "prints the list of all created users", "The 'list' command prints the list of all created users." },
                    new string[] { "edit", "edits existing record", "The 'edit' command edits existing record." },
                    new string[] { "find", "finds all records with specified criterias", "The 'find' command finds all records with specified criterias." },
                    new string[] { "export", "expots all records to the file with specified format", "The 'export' command expots all records to the file with specified format." },
                    new string[] { "import", "imports records from the file with specified format", "The 'import' command imports records from the file with specified format." },
                    new string[] { "remove", "removes record", "The 'remove' command removes record." },
                    new string[] { "purge", "makes defragmentation of the database file", "The 'purge' command makes defragmentation of the database file." },
                    new string[] { "insert", "inserts a new record", "The 'insert' command inserts a new record." },
                    new string[] { "delete", "deletes existing record(s)", "The 'delete' command deletes existing record(s)." },
        };

        /// <summary>
        ///     Handles 'help' command.
        /// </summary>
        /// <param name="commandRequest">Input data for a requested command.</param>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (!commandRequest.Command.Equals("help", StringComparison.InvariantCultureIgnoreCase))
            {
                base.Handle(commandRequest);
                return;
            }

            if (!string.IsNullOrEmpty(commandRequest.Parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], commandRequest.Parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{commandRequest.Parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }
    }
}
