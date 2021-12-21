using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    public class ParameterValidationChain<T>
    {
        private List<Predicate<T>> validationList;

        public ParameterValidationChain()
        {
            this.validationList = new List<Predicate<T>>();
        }

        public bool Validate(T value)
        {
            bool valueIsValid = true;

            foreach (var validationFunc in this.validationList)
            {
                valueIsValid = valueIsValid && validationFunc(value);
            }

            return valueIsValid;
        }

        public ParameterValidationChain<T> AddCondition(Predicate<T> condition)
        {
            Guard.ArgumentIsNotNull(condition, nameof(condition));

            this.validationList.Add(condition);

            return this;
        }
    }
}