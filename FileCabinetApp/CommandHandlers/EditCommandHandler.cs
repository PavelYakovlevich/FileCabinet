using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling of 'edit' command.
    /// </summary>
    public class EditCommandHandler : CommandHandlerBase
    {
        /// <summary>
        ///     Handles 'edit' command.
        /// </summary>
        /// <param name="commandRequest">Input data for a requested command.</param>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (!commandRequest.Command.Equals("edit", StringComparison.InvariantCultureIgnoreCase))
            {
                base.Handle(commandRequest);
                return;
            }

            int id;
            if (!int.TryParse(commandRequest.Parameters, out id))
            {
                Console.WriteLine("Invalid format of id");
                return;
            }

            if (id <= 0)
            {
                Console.WriteLine("Id must be greater than 0.");
                return;
            }

            if (!Program.fileCabinetService.RecordExists(id))
            {
                Console.WriteLine($"#{id} record is not found.");
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

            var parameterObject = new FileCabinetRecordParameterObject(id, firstName, lastName, birthDate, stature, gender, weight);
            Program.fileCabinetService.EditRecord(parameterObject);

            Console.WriteLine($"Record #{id} is updated.");
        }
    }
}
