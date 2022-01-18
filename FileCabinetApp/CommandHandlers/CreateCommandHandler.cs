using System;

using FileCabinetApp.Services;
using FileCabinetApp.Validators;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    ///     Class for handling of 'create' command.
    /// </summary>
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        private readonly IInputValidator inputValidator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service for working with file cabinet records.</param>
        /// <param name="validator">Validator for console input.</param>
        public CreateCommandHandler(IFileCabinetService service, IInputValidator validator)
            : base(service)
        {
            this.inputValidator = validator;
        }

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

            var parameterObject = new FileCabinetRecordParameterObject(firstName, lastName, birthDate, stature, gender, weight);
            var createdRecIndex = this.service.CreateRecord(parameterObject);

            Console.WriteLine($"Record #{createdRecIndex} is created.");
        }
    }
}