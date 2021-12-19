﻿using System;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Pavel Yakovlevich";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static FileCabinetService fileCabinetService = new FileCabinetService();

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints count of registered users", "The 'stat' command prints count of registered users." },
            new string[] { "create", "creates a new user", "The 'create' command creates a new user." },
            new string[] { "list", "prints the list of all created users", "The 'list' command prints the list of all created users." },
            new string[] { "edit", "edits existing record", "The 'edit' command edits existing record." },
            new string[] { "find", "finds all records with specified criterias", "The 'find' command finds all records with specified criterias." },
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine() !.Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            var validationChain = new ParameterValidationChain<string>()
                .AddCondition((value) => !(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value)))
                .AddCondition((value) => value.Length >= 2 && value.Length <= 60);

            var firstName = GuidedParameterReader.ReadValue(
                () => Console.ReadLine() !,
                validationChain,
                "First name: ");

            validationChain = new ParameterValidationChain<string>()
                .AddCondition((value) => !(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value)))
                .AddCondition((value) => value.Length >= 2 && value.Length <= 60);

            var lastName = GuidedParameterReader.ReadValue(
                () => Console.ReadLine() !,
                validationChain,
                "Last name: ");

            var birthDayValidationChain = new ParameterValidationChain<DateTime>()
                .AddCondition((dateOfBirth) => dateOfBirth.CompareTo(new DateTime(1950, 1, 1)) >= 0 && dateOfBirth.CompareTo(DateTime.Now) <= 0);

            var birthDate = GuidedParameterReader.ReadValue(
                () =>
                {
                    DateTime birthDate;
                    DateTime.TryParse(Console.ReadLine(), out birthDate);
                    return birthDate;
                },
                birthDayValidationChain,
                "Date of birth: ");

            var statureValidationChain = new ParameterValidationChain<short>()
                .AddCondition((stature) => stature > 0);

            var stature = GuidedParameterReader.ReadValue(
                () =>
                {
                    short stature;
                    short.TryParse(Console.ReadLine(), out stature);
                    return stature;
                },
                statureValidationChain,
                "Stature: ");

            var weightValidationChain = new ParameterValidationChain<decimal>()
                .AddCondition((weight) => weight > 0);

            var weight = GuidedParameterReader.ReadValue(
                () =>
                {
                    decimal weight;
                    decimal.TryParse(Console.ReadLine(), out weight);
                    return weight;
                },
                weightValidationChain,
                "Weight: ");

            var genderValidationChain = new ParameterValidationChain<char>()
                .AddCondition((gender) => (gender == 'M' || gender == 'F'));

            var gender = GuidedParameterReader.ReadValue(
                () => Console.ReadKey().KeyChar,
                genderValidationChain,
                "Gender: ");

            Console.WriteLine();

            var createdRecIndex = fileCabinetService.CreateRecord(firstName, lastName, birthDate, gender, weight, stature);
            Console.WriteLine($"Record #{createdRecIndex} is created.");
        }

        private static void List(string parameters)
        {
            var records = fileCabinetService.GetRecords();

            foreach (var record in records)
            {
                Console.WriteLine($"#{record}");
            }
        }

        private static void Edit(string parameters)
        {
            int id;
            if (!int.TryParse(parameters, out id))
            {
                Console.WriteLine("Invalid format of id");
                return;
            }

            if (fileCabinetService.GetRecord(id) is null)
            {
                Console.WriteLine($"#{id} record is not found.");
                return;
            }

            var validationChain = new ParameterValidationChain<string>()
                .AddCondition((value) => !(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value)))
                .AddCondition((value) => value.Length >= 2 && value.Length <= 60);

            var firstName = GuidedParameterReader.ReadValue(
                () => Console.ReadLine() !,
                validationChain,
                "First name: ");

            validationChain = new ParameterValidationChain<string>()
                .AddCondition((value) => !(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value)))
                .AddCondition((value) => value.Length >= 2 && value.Length <= 60);

            var lastName = GuidedParameterReader.ReadValue(
                () => Console.ReadLine() !,
                validationChain,
                "Last name: ");

            var birthDayValidationChain = new ParameterValidationChain<DateTime>()
                .AddCondition((dateOfBirth) => dateOfBirth.CompareTo(new DateTime(1950, 1, 1)) >= 0 && dateOfBirth.CompareTo(DateTime.Now) <= 0);

            var birthDate = GuidedParameterReader.ReadValue(
                () =>
                {
                    DateTime birthDate;
                    DateTime.TryParse(Console.ReadLine(), out birthDate);
                    return birthDate;
                },
                birthDayValidationChain,
                "Date of birth: ");

            var statureValidationChain = new ParameterValidationChain<short>()
                .AddCondition((stature) => stature > 0);

            var stature = GuidedParameterReader.ReadValue(
                () =>
                {
                    short stature;
                    short.TryParse(Console.ReadLine(), out stature);
                    return stature;
                },
                statureValidationChain,
                "Stature: ");

            var weightValidationChain = new ParameterValidationChain<decimal>()
                .AddCondition((weight) => weight > 0);

            var weight = GuidedParameterReader.ReadValue(
                () =>
                {
                    decimal weight;
                    decimal.TryParse(Console.ReadLine(), out weight);
                    return weight;
                },
                weightValidationChain,
                "Weight: ");

            var genderValidationChain = new ParameterValidationChain<char>()
                .AddCondition((gender) => (gender == 'M' || gender == 'F'));

            var gender = GuidedParameterReader.ReadValue(
                () =>
                {
                    var result = Console.ReadKey().KeyChar;
                    Console.WriteLine();
                    return result;
                },
                genderValidationChain,
                "Gender: ");

            fileCabinetService.EditRecord(id, firstName, lastName, birthDate, gender, weight, stature);
            Console.WriteLine($"Record #{id} is updated.");
        }

        private static void Find(string parameters)
        {
            var values = parameters.Split(' ', 2);

            if (values.Length < 2)
            {
                Console.WriteLine("Parameter value is missing!");
                return;
            }

            var paramName = values[0];
            var paramValue = values[1].Substring(1, values[1].Length - 2);

            FileCabinetRecord[] records;
            if (paramName.Equals("firstname", StringComparison.InvariantCultureIgnoreCase))
            {
                records = fileCabinetService.FindByFirstName(paramValue);
            }
            else if (paramName.Equals("lastname", StringComparison.InvariantCultureIgnoreCase))
            {
                records = fileCabinetService.FindByLastName(paramValue);
            }
            else
            {
                Console.WriteLine("Search criteria by specified parameter is not defined!");
                return;
            }

            foreach (var record in records)
            {
                Console.WriteLine($"#{record}");
            }
        }
    }
}