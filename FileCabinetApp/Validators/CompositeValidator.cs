using System;
using System.Collections.Generic;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Composite validator.
    /// </summary>
    public class CompositeValidator : FileRecordValidatorBase
    {
        private List<IRecordValidator> validators;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CompositeValidator"/> class.
        /// </summary>
        /// <param name="validators">List of validators, which must be included into internal list of validators.</param>
        public CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            Guard.ArgumentIsNotNull(validators, nameof(validators));

            this.validators = new List<IRecordValidator>(validators);
        }

        /// <summary>
        ///     Validates <see cref="FileCabinetRecord"/> object.
        /// </summary>
        /// <param name="record">Object, which must be validated.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="record"/> is null.</exception>
        public override void Validate(FileCabinetRecord record)
        {
            Guard.ArgumentIsNotNull(record, nameof(record));

            foreach (var validator in this.validators)
            {
                validator.Validate(record);
            }
        }
    }
}