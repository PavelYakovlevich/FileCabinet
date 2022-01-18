using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FileCabinetApp.Utils
{
    /// <summary>
    ///     Class for comparing <typeparamref name="T"/> objects.
    /// </summary>
    /// <typeparam name="T">Type of comparable objects.</typeparam>
    public class SearchInfoComparer<T> : IEqualityComparer<SearchInfo<T>>
    {
        /// <summary>
        ///     Compares <paramref name="obj1"/> and <paramref name="obj2"/>.
        /// </summary>
        /// <param name="obj1">First object, which will be compared with <paramref name="obj2"/>.</param>
        /// <param name="obj2">Second object, which will be compared with <paramref name="obj1"/>.</param>
        /// <returns>True of <paramref name="obj2"/> and <paramref name="obj2"/> are equal.</returns>
        public bool Equals(SearchInfo<T>? obj1, SearchInfo<T>? obj2)
        {
            if (obj1 is null && obj2 is null)
            {
                return false;
            }

            if (obj1 is null || obj2 is null)
            {
                return false;
            }

            if (object.ReferenceEquals(obj1, obj2))
            {
                return true;
            }

            foreach (var searchCriteria in obj2.SearchCriterias)
            {
                var searchCriteriaName = searchCriteria.Key;

                if (!obj1.SearchCriterias.ContainsKey(searchCriteriaName))
                {
                    return false;
                }

                var expectedCriteriasValues = obj1.SearchCriterias[searchCriteriaName];
                var actualCriteriasValues = searchCriteria.Value;

                if (expectedCriteriasValues.Count != actualCriteriasValues.Count)
                {
                    return false;
                }

                foreach (var actualCriteriaValue in actualCriteriasValues)
                {
                    if (!expectedCriteriasValues.Contains(actualCriteriaValue))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        ///     Computes hashcode of the <paramref name="obj"/> object.
        /// </summary>
        /// <param name="obj">Object, for which hashcode will be computed.</param>
        /// <returns>Hashcode of the <paramref name="obj"/> object.</returns>
        public int GetHashCode([DisallowNull] SearchInfo<T> obj)
        {
            const int seed = 39;
            var hashCode = 0;

            unchecked
            {
                foreach (var searchCriteria in obj.SearchCriterias)
                {
                    var searchCriteriaName = searchCriteria.Key;
                    hashCode += seed * searchCriteriaName.GetHashCode();

                    var criteriasValues = searchCriteria.Value;

                    foreach (var criteriaValue in criteriasValues)
                    {
                        hashCode += seed * criteriaValue.GetHashCode();
                    }
                }
            }

            return hashCode;
        }
    }
}