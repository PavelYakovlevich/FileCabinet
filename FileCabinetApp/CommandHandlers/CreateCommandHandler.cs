using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling of 'create' command.
    /// </summary>
    public class CreateCommandHandler : CommandHandlerBase
    {
        /// <summary>
        ///     Handles 'create' command.
        /// </summary>
        /// <param name="commandRequest">Input data for a requested command.</param>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (!commandRequest.Command.Equals("create", StringComparison.InvariantCultureIgnoreCase))
            {
                base.Handle(commandRequest);
                return;
            }

            Console.Write("First name: ");
            var firstName = ConsoleParameterReader.ReadInput(InputUtils.StringConverter, Program.consoleInputValidator.ValidateFirstName);

            Console.Write("Last name: ");
            var lastName = ConsoleParameterReader.ReadInput(InputUtils.StringConverter, Program.consoleInputValidator.ValidateLastName);

            Console.Write("Date of birth: ");
            var birthDate = ConsoleParameterReader.ReadInput(InputUtils.DateTimeConverter, Program.consoleInputValidator.ValidateBirthDay);

            Console.Write("Stature: ");
            var stature = ConsoleParameterReader.ReadInput(InputUtils.ShortConverter, Program.consoleInputValidator.ValidateStature);

            Console.Write("Weight: ");
            var weight = ConsoleParameterReader.ReadInput(InputUtils.DecimalConverter, Program.consoleInputValidator.ValidateWeight);

            Console.Write("Gender: ");
            var gender = ConsoleParameterReader.ReadInput(InputUtils.CharConverter, Program.consoleInputValidator.ValidateGender);

            var parameterObject = new FileCabinetRecordParameterObject(firstName, lastName, birthDate, stature, gender, weight);
            var createdRecIndex = Program.fileCabinetService.CreateRecord(parameterObject);

            Console.WriteLine($"Record #{createdRecIndex} is created.");
        }
    }
}
