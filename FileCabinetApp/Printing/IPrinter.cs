using System;
using System.Collections.Generic;

namespace FileCabinetApp.Printing
{
    /// <summary>
    ///     Interface for printers of <see cref="FileCabinetApp"/> objects.
    /// </summary>
    /// <typeparam name="T">Type of printable object.</typeparam>
    public interface IPrinter<T>
    {
        /// <summary>
        ///     Prints object's data.
        /// </summary>
        /// <param name="obj">Object to print.</param>
        void Print(T obj);

        /// <summary>
        ///     Prints all <paramref name="objects"/>.
        /// </summary>
        /// <param name="objects">Objects to print.</param>
        void Print(IEnumerable<T> objects);

        /// <summary>
        ///     Prints all <paramref name="objects"/>.
        /// </summary>
        /// <param name="objects">Objects to print.</param>
        /// <param name="selectors">Selectors of the object's values.</param>
        void Print(IEnumerable<T> objects, IDictionary<string, (Func<T, string> selector, ValuePadding padding)> selectors);
    }
}