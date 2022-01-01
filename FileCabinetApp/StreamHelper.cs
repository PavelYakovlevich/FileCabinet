using System;
using System.IO;
using System.Text;
using FileCabinetApp.Utils;

namespace FileCabinetApp
{
    /// <summary>
    ///     Class for reading, writing an primitive objects from, to a stream.
    /// </summary>
    public static class StreamHelper
    {
        /// <summary>
        ///     Read a <see cref="string"/> object from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object, from which a string will be read.</param>
        /// <param name="maxSize">Maximum size of string.</param>
        /// <returns><see cref="string"/> object, which was read from a <see cref="Stream"/>.</returns>
        public static string ReadString(Stream stream, int maxSize)
        {
            var buffer = new byte[maxSize];

            stream.Read(buffer, 0, maxSize);

            return Encoding.Default.GetString(buffer);
        }

        /// <summary>
        ///     Read a <see cref="int"/> object from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object, from which a <see cref="int"/> will be read.</param>
        /// <returns><see cref="int"/>, which was read from a <see cref="Stream"/>.</returns>
        public static int ReadInt(Stream stream)
        {
            var buffer = new byte[sizeof(int)];

            stream.Read(buffer);

            return BitConverter.ToInt32(buffer);
        }

        /// <summary>
        ///     Read a <see cref="short"/> object from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object, from which a <see cref="short"/> will be read.</param>
        /// <returns><see cref="short"/>, which was read from a <see cref="Stream"/>.</returns>
        public static short ReadShort(Stream stream)
        {
            var buffer = new byte[sizeof(short)];

            stream.Read(buffer);

            return BitConverter.ToInt16(buffer);
        }

        /// <summary>
        ///     Read a <see cref="decimal"/> from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object, from which a <see cref="decimal"/> will be read.</param>
        /// <returns><see cref="decimal"/>, which was read from a <see cref="Stream"/>.</returns>
        public static decimal ReadDecimal(Stream stream)
        {
            var buffer = new byte[sizeof(decimal)];

            stream.Read(buffer);

            return BitConverterExtension.ToDecimal(buffer);
        }

        /// <summary>
        ///     Read a <see cref="char"/> from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object, from which a <see cref="char"/> will be read.</param>
        /// <returns><see cref="char"/> object, which was read from a <see cref="Stream"/>.</returns>
        public static char ReadChar(Stream stream)
        {
            var buffer = new byte[sizeof(char)];

            stream.Read(buffer);

            return BitConverter.ToChar(buffer);
        }

        /// <summary>
        ///     Read a <see cref="DateTime"/> object from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object, from which a <see cref="DateTime"/> will be read.</param>
        /// <returns><see cref="DateTime"/>, which was read from a <see cref="Stream"/>.</returns>
        public static DateTime ReadDateTime(Stream stream)
        {
            var buffer = new byte[sizeof(int) * 3];

            stream.Read(buffer);

            var year = BitConverter.ToInt32(buffer, 0);
            var month = BitConverter.ToInt32(buffer, sizeof(int));
            var day = BitConverter.ToInt32(buffer, 2 * sizeof(int));

            return new DateTime(year, month, day);
        }

        /// <summary>
        ///     Writes an <paramref name="value"/> to a <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object, to which a <paramref name="value"/> will be written.</param>
        /// <param name="value">value, which will ne written to a <paramref name="stream"/>.</param>
        public static void Write(Stream stream, int value)
        {
            var buffer = BitConverter.GetBytes(value);

            stream.Write(buffer);
        }

        /// <summary>
        ///     Writes an <paramref name="value"/> to a <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object, to which a <paramref name="value"/> will be written.</param>
        /// <param name="value">value, which will ne written to a <paramref name="stream"/>.</param>
        public static void Write(Stream stream, short value)
        {
            var buffer = BitConverter.GetBytes(value);

            stream.Write(buffer);
        }

        /// <summary>
        ///     Writes an <paramref name="value"/> to a <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object, to which a <paramref name="value"/> will be written.</param>
        /// <param name="value">Value, which will ne written to a <paramref name="stream"/>.</param>
        public static void Write(Stream stream, char value)
        {
            var buffer = BitConverter.GetBytes(value);

            stream.Write(buffer);
        }

        /// <summary>
        ///     Writes an <paramref name="value"/> to a <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object, to which a <paramref name="value"/> will be written.</param>
        /// <param name="value">Value, which will ne written to a <paramref name="stream"/>.</param>
        public static void Write(Stream stream, decimal value)
        {
            var buffer = BitConverterExtension.GetBytes(value);

            stream.Write(buffer);
        }

        /// <summary>
        ///     Writes an <paramref name="value"/> to a <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object, to which a <paramref name="value"/> will be written.</param>
        /// <param name="value">Value, which will ne written to a <paramref name="stream"/>.</param>
        public static void Write(Stream stream, DateTime value)
        {
            var year = BitConverter.GetBytes(value.Year);
            var month = BitConverter.GetBytes(value.Month);
            var day = BitConverter.GetBytes(value.Day);

            stream.Write(year);
            stream.Write(month);
            stream.Write(day);
        }

        /// <summary>
        ///     Writes an <paramref name="value"/> to a <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object, to which a <paramref name="value"/> will be written.</param>
        /// <param name="value">String, which will ne written to a <paramref name="stream"/>.</param>
        /// <param name="maxSize">Length of a <paramref name="value"/>.</param>
        public static void Write(Stream stream, string value, int maxSize)
        {
            var buffer = Encoding.Default.GetBytes(value, 0, maxSize);

            stream.Write(buffer);
        }
    }
}
