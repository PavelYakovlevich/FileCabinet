using System;

using FileCabinetApp.Services;
using FileCabinetApp.Validators;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling of 'edit' command.
    /// </summary>
    public class EditCommandHandler : CommandHandlerBase
    {
        private readonly IFileCabinetService service;
        private readonly IConsoleInputValidator inputValidator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EditCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service for working with file cabinet records.</param>
        /// <param name="validator">Validator for console input.</param>
        public EditCommandHandler(IFileCabinetService service, IConsoleInputValidator validator)
        {
            this.service = service;
            this.inputValidator = validator;
        }

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

            if (!this.service.RecordExists(id))
            {
                Console.WriteLine($"#{id} record is not found.");
                return;
            }

            Console.Write("First name: ");
            var firstName = ConsoleParameterReader.ReadInput(InputUtils.StringConverter, this.inputValidator.ValidateFirstName);

            Console.Write("Last name: ");
            var lastName = ConsoleParameterReader.ReadInput(InputUtils.StringConverter, this.inputValidator.ValidateLastName);

            Console.Write("Date of birth: ");
            var birthDate = ConsoleParameterReader.ReadInput(InputUtils.DateTimeConverter, this.inputValidator.ValidateBirthDay);

            Console.Write("Stature: ");
            var stature = ConsoleParameterReader.ReadInput(InputUtils.ShortConverter, this.inputValidator.ValidateStature);

            Console.Write("Weight: ");
            var weight = ConsoleParameterReader.ReadInput(InputUtils.DecimalConverter, this.inputValidator.ValidateWeight);

            Console.Write("Gender: ");
            var gender = ConsoleParameterReader.ReadInput(InputUtils.CharConverter, this.inputValidator.ValidateGender);

            var parameterObject = new FileCabinetRecordParameterObject(id, firstName, lastName, birthDate, stature, gender, weight);
            this.service.EditRecord(parameterObject);

            Console.WriteLine($"Record #{id} is updated.");
        }
    }
}
