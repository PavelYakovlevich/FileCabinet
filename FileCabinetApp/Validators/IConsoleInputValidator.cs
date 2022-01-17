using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Interface for the console input validators.
    /// </summary>
    public interface IConsoleInputValidator
    {
        /// <summary>
        ///     Validates <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>(true,"") if <paramref name="id"/> is valid, overwise (false,<see cref="string"/> error message).</returns>
        Tuple<bool, string> ValidateId(int id);

        /// <summary>
        ///     Validates <paramref name="firstName"/>.
        /// </summary>
        /// <param name="firstName">Firstname.</param>
        /// <returns>(true,"") if <paramref name="firstName"/> is valid, overwise (false,<see cref="string"/> error message).</returns>
        Tuple<bool, string> ValidateFirstName(string firstName);

        /// <summary>
        ///     Validates <paramref name="lastName"/>.
        /// </summary>
        /// <param name="lastName">Lastname.</param>
        /// <returns>(true,"") if <paramref name="lastName"/> is valid, overwise (false,<see cref="string"/> error message).</returns>
        Tuple<bool, string> ValidateLastName(string lastName);

        /// <summary>
        ///     Validates <paramref name="dateOfBirth"/>.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <returns>(true,"") if <paramref name="dateOfBirth"/> is valid, overwise (false,<see cref="string"/> error message).</returns>
        Tuple<bool, string> ValidateBirthDay(DateTime dateOfBirth);

        /// <summary>
        ///     Validates <paramref name="stature"/>.
        /// </summary>
        /// <param name="stature">Stature.</param>
        /// <returns>(true,"") if <paramref name="stature"/> is valid, overwise (false,<see cref="string"/> error message).</returns>
        Tuple<bool, string> ValidateStature(short stature);

        /// <summary>
        ///     Validates <paramref name="gender"/>.
        /// </summary>
        /// <param name="gender">Gender.</param>
        /// <returns>(true,"") if <paramref name="gender"/> is valid, overwise (false,<see cref="string"/> error message).</returns>
        Tuple<bool, string> ValidateGender(char gender);

        /// <summary>
        ///     Validates <paramref name="weight"/>.
        /// </summary>
        /// <param name="weight">Weight.</param>
        /// <returns>(true,"") if <paramref name="weight"/> is valid, overwise (false,<see cref="string"/> error message).</returns>
        Tuple<bool, string> ValidateWeight(decimal weight);
    }
}
