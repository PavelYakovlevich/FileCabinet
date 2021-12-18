using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    public class ParameterValidationChain<T>
    {
        private List<Predicate<T>> validationList;
        private Guard guard;

        public ParameterValidationChain()
        {
            this.validationList = new List<Predicate<T>>();
            this.guard = new Guard();
        }

        public bool Validate(T value)
        {
            bool result = true;

            foreach (var validationFunc in this.validationList)
            {
                result = result && validationFunc(value);
            }

            return result;
        }

        public ParameterValidationChain<T> AddCondition(Predicate<T> condition)
        {
            this.guard.IsNotNull(condition, nameof(condition));

            this.validationList.Add(condition);

            return this;
        }
    }
}