using System;
using System.Collections.Generic;

namespace FileCabinetApp.Utils
{
    /// <summary>
    ///     Class with methods for converting <see cref="decimal"/> to bytes array and vice versa.
    /// </summary>
    public static class BitConverterExtension
    {
        /// <summary>
        ///     Converts bytes array <paramref name="bytes"/> to a decimal.
        /// </summary>
        /// <param name="bytes">Byte array, which will be converted to a <see cref="decimal"/>.</param>
        /// <returns><see cref="decimal"/> converted from a <paramref name="bytes"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="bytes"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="bytes"/> length is not equal to 16.</exception>
        public static decimal ToDecimal(byte[] bytes)
        {
            if (bytes is null)
            {
                throw new ArgumentNullException(nameof(bytes), $"{nameof(bytes)} is null.");
            }

            if (bytes.Length != sizeof(decimal))
            {
                throw new ArgumentException($"A decimal must be created from {sizeof(decimal)} bytes exactly.", nameof(bytes));
            }

            int[] bits = new int[4];

            for (int i = 0; i < sizeof(decimal) - 1; i += 4)
            {
                bits[i / 4] = BitConverter.ToInt32(bytes, i);
            }

            return new decimal(bits);
        }

        /// <summary>
        ///     Converts <paramref name="dec"/> to bytes array.
        /// </summary>
        /// <param name="dec"><see cref="decimal"/>, which will be converted bytes array.</param>
        /// <returns><see cref="Array"/> of bytes with the converted <paramref name="dec"/>.</returns>
        public static byte[] GetBytes(decimal dec)
        {
            var bits = decimal.GetBits(dec);

            var bytes = new List<byte>();
            foreach (int i in bits)
            {
                bytes.AddRange(BitConverter.GetBytes(i));
            }

            return bytes.ToArray();
        }
    }
}
