using System;

namespace FileCabinetApp.Configuration
{
    public class ValidationConfig
    {
        public StringConstraint FirstName { get; set; }

        public StringConstraint LastName { get; set; }

        public DateTimeRangeConstraint DateOfBirth { get; set; }

        public NumericRangeConstraint<short> Stature { get; set; }

        public NumericRangeConstraint<decimal> Weight { get; set; }

        public char[] Gender { get; set; }

    }

    public class StringConstraint 
    {
        public int MinLength { get; set; } = 0;

        public int MaxLength { get; set; } = int.MaxValue;
    }

    public class NumericRangeConstraint<T>
    {
        public T MinValue { get; set; }

        public T MaxValue { get; set; }
    }

    public class DateTimeRangeConstraint
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
