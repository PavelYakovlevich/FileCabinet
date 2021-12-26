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
    }
}
