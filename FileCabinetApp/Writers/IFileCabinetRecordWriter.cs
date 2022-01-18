namespace FileCabinetApp.Writers
{
    /// <summary>
    ///     Interface for file cabinet records writers.
    /// </summary>
    public interface IFileCabinetRecordWriter
    {
        /// <summary>
        ///     Writes <see cref="FileCabinetRecord"/> to a stream.
        /// </summary>
        /// <param name="record"><see cref="FileCabinetRecord"/> object, which must be written to a stream.</param>
        void Write(FileCabinetRecord record);
    }
}