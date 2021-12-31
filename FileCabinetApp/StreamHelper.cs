using System;
using System.IO;
using System.Text;
using FileCabinetApp.Utils;

namespace FileCabinetApp
{
    public static class StreamHelper
    {
        public static string ReadString(Stream stream, int maxSize)
        {
            var buffer = new byte[maxSize];

            stream.Read(buffer, 0, maxSize);

            return Encoding.Default.GetString(buffer);
        }

        public static int ReadInt(Stream stream)
        {
            var buffer = new byte[sizeof(int)];

            stream.Read(buffer);

            return BitConverter.ToInt32(buffer);
        }

        public static short ReadShort(Stream stream)
        {
            var buffer = new byte[sizeof(short)];

            stream.Read(buffer);

            return BitConverter.ToInt16(buffer);
        }

        public static decimal ReadDecimal(Stream stream)
        {
            var buffer = new byte[sizeof(decimal)];

            stream.Read(buffer);

            return BitConverterExtension.ToDecimal(buffer);
        }

        public static char ReadChar(Stream stream)
        {
            var buffer = new byte[sizeof(char)];

            stream.Read(buffer);

            return BitConverter.ToChar(buffer);
        }

        public static DateTime ReadDateTime(Stream stream)
        {
            var buffer = new byte[sizeof(int) * 3];

            stream.Read(buffer);

            var year = BitConverter.ToInt32(buffer, 0);
            var month = BitConverter.ToInt32(buffer, sizeof(int));
            var day = BitConverter.ToInt32(buffer, 2 * sizeof(int));

            return new DateTime(year, month, day);
        }

        public static void Write(Stream stream, int value)
        {
            var buffer = BitConverter.GetBytes(value);

            stream.Write(buffer);
        }

        public static void Write(Stream stream, short value)
        {
            var buffer = BitConverter.GetBytes(value);

            stream.Write(buffer);
        }

        public static void Write(Stream stream, char value)
        {
            var buffer = BitConverter.GetBytes(value);

            stream.Write(buffer);
        }

        public static void Write(Stream stream, decimal value)
        {
            var buffer = BitConverterExtension.GetBytes(value);

            stream.Write(buffer);
        }

        public static void Write(Stream stream, DateTime value)
        {
            var year = BitConverter.GetBytes(value.Year);
            var month = BitConverter.GetBytes(value.Month);
            var day = BitConverter.GetBytes(value.Day);

            stream.Write(year);
            stream.Write(month);
            stream.Write(day);
        }

        public static void Write(Stream stream, string value, int maxSize)
        {
            var buffer = Encoding.Default.GetBytes(value, 0, maxSize);

            stream.Write(buffer);
        }
    }
}
