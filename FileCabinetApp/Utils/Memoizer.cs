using System;
using System.Collections.Generic;

#pragma warning disable CS8600

namespace FileCabinetApp.Utils
{
    /// <summary>
    ///     Class for saving results of functions.
    /// </summary>
    /// <typeparam name="TParam">Type of saving key.</typeparam>
    /// <typeparam name="TValue">Type of the return values.</typeparam>
    public class Memoizer<TParam, TValue>
        where TParam : notnull
    {
        private readonly IEqualityComparer<TParam> equalityComparer;
        private readonly Dictionary<TParam, TValue> cache;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Memoizer{TParam, TValue}"/> class.
        /// </summary>
        /// <param name="equalityComparer"><see cref="IEqualityComparer{T}"/> object, which perfoms comparing of key objects.</param>
        public Memoizer(IEqualityComparer<TParam> equalityComparer)
        {
            Guard.ArgumentIsNotNull(equalityComparer, nameof(equalityComparer));

            this.equalityComparer = equalityComparer;
            this.cache = new Dictionary<TParam, TValue>(equalityComparer);
        }

        /// <summary>
        ///     Perfoms memoryzation of <paramref name="func"/> execution result.
        /// </summary>
        /// <param name="func">Function, which result must be memorized.</param>
        /// <returns>Function, which will return memorized value, if it was saved in cache or will execure <paramref name="func"/> and save it's result to the internal objects cache.</returns>
        public Func<TParam, TValue> Memoize(Func<TParam, TValue> func)
        {
            return (parameters) =>
            {
                TValue result = default;

                if (!this.cache.TryGetValue(parameters, out result))
                {
                    result = func(parameters);

                    this.cache.Add(parameters, result);
                }

                return result;
            };
        }

        /// <summary>
        ///     Clears the internal objects cache.
        /// </summary>
        public void ClearCache()
        {
            this.cache.Clear();
        }
    }
}