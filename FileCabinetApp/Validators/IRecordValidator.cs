namespace FileCabinetApp
{
    /// <summary>
    ///     Interface for the input parameters validators.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        ///     Validates <see cref="FileCabinetRecordParameterObject"/> object.
        /// </summary>
        /// <param name="parameterObject">Parameter object.</param>
        void ValidateParameters(FileCabinetRecordParameterObject parameterObject);

        /// <summary>
        ///     Validates <see cref="FileCabinetRecord"/> object.
        /// </summary>
        /// <param name="parameterObject">File cabinet record object.</param>
        void Validate(FileCabinetRecord parameterObject);
    }
}