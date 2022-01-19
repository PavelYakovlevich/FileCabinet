using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileCabinetApp.Printing
{
    /// <summary>
    ///     Class for printing an objects in a table form.
    /// </summary>
    /// <typeparam name="T">Type of printable objects.</typeparam>
    public class TablePrinter<T> : IPrinter<T>
        where T : notnull
    {
        private static readonly char LineConnetor = '+';
        private static readonly char VerticalLine = '|';
        private static readonly char HorizontalLine = '-';

        private static readonly int FieldTerminatingSpacesCount = 2;

        /// <inheritdoc cref="IPrinter{T}.Print(IEnumerable{T})"/>
        public void Print(IEnumerable<T> objects)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IPrinter{T}.Print(T)"/>
        public void Print(T obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Prints all <paramref name="objects"/>.
        /// </summary>
        /// <param name="objects">Objects to print.</param>
        /// <param name="selectors">Selectors of the object's values.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="objects"/> or <paramref name="selectors"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="selectors"/> is empty.</exception>
        public void Print(IEnumerable<T> objects, IDictionary<string, (Func<T, string> selector, ValuePadding padding)> selectors)
        {
            Guard.ArgumentIsNotNull(objects, nameof(objects));

            Guard.ArgumentIsNotNull(selectors, nameof(selectors));
            Guard.ArgumentGreaterThan(selectors.Count, 0, $"{nameof(selectors)} is empty.");

            if (Enumerable.Count(objects) == 0)
            {
                Console.WriteLine("There is no records to display.");
                return;
            }

            var columnsLengthes = this.GetColumnsLengthes(objects, selectors);

            var separatorLine = this.GetSeparatorLine(columnsLengthes);

            var header = this.GetHeader(columnsLengthes, separatorLine, selectors);
            Console.Write(header);

            foreach (var obj in objects)
            {
                this.PrintObject(obj, columnsLengthes, separatorLine, selectors);
            }
        }

        private string GetHeader(int[] columnsLengthes, string separatorLine, IDictionary<string, (Func<T, string> selector, ValuePadding padding)> selectors)
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine(separatorLine).Append(VerticalLine);

            int currentColumnLengthIndex = 0;

            foreach (var selector in selectors)
            {
                var fieldName = selector.Key;
                var currentColumnLength = columnsLengthes[currentColumnLengthIndex++] + FieldTerminatingSpacesCount;

                result.Append(fieldName.PadRight(currentColumnLength)).Append(VerticalLine);
            }

            result.AppendLine().AppendLine(separatorLine);

            return result.ToString();
        }

        private int[] GetColumnsLengthes(IEnumerable<T> objects, IDictionary<string, (Func<T, string> selector, ValuePadding padding)> selectors)
        {
            var result = new int[selectors.Count];
            var currentFieldLengthIndex = 0;
            foreach (var selector in selectors)
            {
                result[currentFieldLengthIndex++] = selector.Key.Length;
            }

            foreach (var obj in objects)
            {
                currentFieldLengthIndex = 0;

                foreach (var selector in selectors)
                {
                    var selectorFunc = selector.Value.selector;

                    var fieldValueLength = selectorFunc(obj).Length;

                    if (result[currentFieldLengthIndex] < fieldValueLength)
                    {
                        result[currentFieldLengthIndex] = fieldValueLength;
                    }

                    currentFieldLengthIndex++;
                }
            }

            return result;
        }

        private string GetSeparatorLine(int[] fieldsLengthes)
        {
            StringBuilder result = new StringBuilder();

            result.Append(LineConnetor.ToString());

            foreach (var fieldLength in fieldsLengthes)
            {
                result.Append(HorizontalLine, fieldLength + FieldTerminatingSpacesCount);
                result.Append(LineConnetor);
            }

            return result.ToString();
        }

        private void PrintObject(T obj, int[] columnsLengthes, string separatorLine, IDictionary<string, (Func<T, string> selector, ValuePadding padding)> selectors)
        {
            var objectString = new StringBuilder();
            objectString.Append(VerticalLine);

            var currentFieldIndex = 0;
            foreach (var selector in selectors)
            {
                var columnLength = columnsLengthes[currentFieldIndex++];

                var selectorFunc = selector.Value.selector;

                var fielValue = selectorFunc(obj);

                var paddingDirection = selector.Value.padding;

                objectString.Append(' ', FieldTerminatingSpacesCount / 2);
                switch (paddingDirection)
                {
                    case ValuePadding.Left:
                        objectString.Append(fielValue.PadLeft(columnLength));
                        break;
                    case ValuePadding.Right:
                        objectString.Append(fielValue.PadRight(columnLength));
                        break;
                }

                objectString.Append(' ', FieldTerminatingSpacesCount / 2).Append(VerticalLine);
            }

            Console.WriteLine(objectString);
            Console.WriteLine(separatorLine);
        }
    }
}