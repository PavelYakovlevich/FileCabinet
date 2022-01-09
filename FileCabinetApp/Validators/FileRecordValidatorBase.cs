namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Abstract base class for all <see cref="FileCabinetRecord"/>'s validators.
    /// </summary>
    public abstract class FileRecordValidatorBase : IRecordValidator
    {
        /// <inheritdoc cref="IRecordValidator.Validate(FileCabinetRecord)"/>
        public abstract void Validate(FileCabinetRecord record);

        /// <inheritdoc cref="IRecordValidator.ValidateParameters(FileCabinetRecordParameterObject)"/>
        public void ValidateParameters(FileCabinetRecordParameterObject parameterObject)
        {
            var record = new FileCabinetRecord
            {
                Id = parameterObject.Id,
                FirstName = parameterObject.FirstName,
                LastName = parameterObject.LastName,
                DateOfBirth = parameterObject.DateOfBirth,
                Gender = parameterObject.Gender,
                Weight = parameterObject.Weight,
                Stature = parameterObject.Stature,
            };

            this.Validate(record);
        }
    }
}
