using System;
using System.Collections.Generic;

namespace FileCabinetApp.Utils
{
    public static class BitConverterExtension
    {
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
