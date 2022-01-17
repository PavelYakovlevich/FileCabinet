using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    /// <summary>
    ///     Class for holding info for the searching.
    /// </summary>
    /// <typeparam name="T">Type of a searchable object.</typeparam>
    public class SearchInfo<T>
    {
        private readonly IDictionary<string, Func<string, Tuple<bool, string, Predicate<T>?>>> conditionCreators;

        private SearchInfo(IDictionary<string, Func<string, Tuple<bool, string, Predicate<T>?>>> conditionCreators)
        {
            Guard.ArgumentIsNotNull(conditionCreators, nameof(conditionCreators));

            this.conditionCreators = conditionCreators;
            this.SearchCriterias = new Dictionary<string, IList<string>>();
            this.SearchPredicate = (record) => true;
        }

        private enum ExpectedLogicalOperator
        {
            None,
            And,
            Or,
        }

        /// <summary>
        ///     Gets dictionary with all search criterias.
        /// </summary>
        /// <value>Dictionary with all search criterias.</value>
        public Dictionary<string, IList<string>> SearchCriterias { get; private set; }

        /// <summary>
        ///     Gets search condition.
        /// </summary>
        /// <value>Search condition.</value>
        public Predicate<T> SearchPredicate { get; private set; }

        /// <summary>
        ///     Parses an <paramref name="searchCriteriasPairs"/> and creates an instance of the <see cref="SearchInfo{T}"/> class.
        /// </summary>
        /// <param name="conditionCreators"><see cref="IDictionary{TKey, TValue}"/> object with methods, which are responsible for creating of predicates according to the found fields. </param>
        /// <param name="searchCriteriasPairs"><see cref="Array"/> object, which must be parsed.</param>
        /// <returns>Created instance of the <see cref="SearchInfo{T}"/> class.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="conditionCreators"/> or <paramref name="searchCriteriasPairs"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="conditionCreators"/> is empty.</exception>
        public static SearchInfo<T> Create(IDictionary<string, Func<string, Tuple<bool, string, Predicate<T>?>>> conditionCreators, string[] searchCriteriasPairs)
        {
            Guard.ArgumentIsNotNull(searchCriteriasPairs, nameof(searchCriteriasPairs));
            Guard.ArgumentGreaterThan(searchCriteriasPairs.Length, 0, $"{nameof(searchCriteriasPairs)} is empty.");

            var result = new SearchInfo<T>(conditionCreators);

            var expectedLogicalOperator = ExpectedLogicalOperator.None;

            int currentSearchCriteriaIndex = 0;
            while (currentSearchCriteriaIndex < searchCriteriasPairs.Length)
            {
                var fieldName = searchCriteriasPairs[currentSearchCriteriaIndex].ToLower();

                if (fieldName.Equals("and"))
                {
                    if (expectedLogicalOperator != ExpectedLogicalOperator.None)
                    {
                        throw new ArgumentException($"Logical operator {fieldName} can't follow another logical operator.");
                    }

                    expectedLogicalOperator = ExpectedLogicalOperator.And;
                    currentSearchCriteriaIndex++;

                    continue;
                }

                if (fieldName.Equals("or"))
                {
                    if (expectedLogicalOperator != ExpectedLogicalOperator.None)
                    {
                        throw new ArgumentException($"Logical operator {fieldName} can't follow another logical operator.");
                    }

                    expectedLogicalOperator = ExpectedLogicalOperator.Or;
                    currentSearchCriteriaIndex++;

                    continue;
                }

                if (!result.conditionCreators.ContainsKey(fieldName))
                {
                    throw new ArgumentException($"Unknown '{fieldName}' field.");
                }

                if (currentSearchCriteriaIndex + 1 >= searchCriteriasPairs.Length)
                {
                    throw new ArgumentException($"Missing value for the {fieldName} field.");
                }

                var fieldValue = searchCriteriasPairs[currentSearchCriteriaIndex + 1].Trim('\'');

                var creationResult = result.conditionCreators[fieldName](fieldValue);

                if (!creationResult.Item1)
                {
                    throw new ArgumentException(creationResult.Item2);
                }

                result.UpdateSearchCriterias(fieldName, fieldValue);

                var nextCondition = creationResult.Item3!;

                result.SearchPredicate = expectedLogicalOperator switch
                {
                    ExpectedLogicalOperator.And => And(result.SearchPredicate, nextCondition),
                    ExpectedLogicalOperator.Or => Or(result.SearchPredicate, nextCondition),
                    _ => result.SearchPredicate = nextCondition,
                };

                expectedLogicalOperator = ExpectedLogicalOperator.None;
                currentSearchCriteriaIndex += 2;
            }

            if (expectedLogicalOperator != ExpectedLogicalOperator.None)
            {
                throw new ArgumentException($"Missing condition after a {expectedLogicalOperator}.");
            }

            return result;
        }

        private static Predicate<T> Or(params Predicate<T>[] conditions)
        {
            return (obj) =>
            {
                foreach (var condition in conditions)
                {
                    if (condition(obj))
                    {
                        return true;
                    }
                }

                return false;
            };
        }

        private static Predicate<T> And(params Predicate<T>[] conditions)
        {
            return (obj) =>
            {
                foreach (var condition in conditions)
                {
                    if (!condition(obj))
                    {
                        return false;
                    }
                }

                return true;
            };
        }

        private void UpdateSearchCriterias(string searchCriteriaName, string searchCriteriaValue)
        {
            if (!this.SearchCriterias.ContainsKey(searchCriteriaName))
            {
                this.SearchCriterias.Add(searchCriteriaName, new List<string>());
            }

            this.SearchCriterias[searchCriteriaName].Add(searchCriteriaValue);
        }
    }
}
