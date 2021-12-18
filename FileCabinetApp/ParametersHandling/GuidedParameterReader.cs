using System;

namespace FileCabinetApp
{
    public static class GuidedParameterReader
    {
        public static string ErrorMessage { get; set; } = "Value is not valid. Please, try again.";

        public static T ReadValue<T>(Func<T> readerFunc, ParameterValidationChain<T> validationChain, string paramInputMessage)
        {
            return ReadValue(readerFunc, validationChain, paramInputMessage, null);
        }

        public static T ReadValue<T>(Func<T> readerFunc, ParameterValidationChain<T> validationChain, string paramInputMessage, string? hintMessage)
        {
            var guard = new Guard();

            guard.IsNotNull(readerFunc, nameof(readerFunc));
            guard.IsNotNull(validationChain, nameof(validationChain));

            T value;
            bool validationRes;
            bool hintMessageIsDefined = string.IsNullOrEmpty(hintMessage) || !string.IsNullOrWhiteSpace(hintMessage);
            do
            {
                Console.Write(paramInputMessage);
                value = readerFunc();

                validationRes = validationChain.Validate(value);
                if (!validationRes)
                {
                    Console.WriteLine(ErrorMessage);

                    if (!hintMessageIsDefined)
                    {
                        Console.WriteLine($"Hint: ${hintMessage}");
                    }
                }
            }
            while (!validationRes);

            return value;
        }
    }
}
