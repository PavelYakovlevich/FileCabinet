using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
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

        public Dictionary<string, IList<string>> SearchCriterias { get; private set; }

        public Predicate<T> SearchPredicate { get; private set; }

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

        private void UpdateSearchCriterias(string searchCriteriaName, string searchCriteriaValue)
        {
            if (!this.SearchCriterias.ContainsKey(searchCriteriaName))
            {
                this.SearchCriterias.Add(searchCriteriaName, new List<string>());
            }

            this.SearchCriterias[searchCriteriaName].Add(searchCriteriaValue);
        }

        private static Predicate<T> Or(params Predicate<T>[] conditions)
        {
            return (obj) =>
            {
                foreach (var condition in conditions)
                {
                    Guard.ArgumentIsNotNull(condition, nameof(condition));

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
                    Guard.ArgumentIsNotNull(condition, nameof(condition));

                    if (!condition(obj))
                    {
                        return false;
                    }
                }

                return true;
            };
        }
    }
}
