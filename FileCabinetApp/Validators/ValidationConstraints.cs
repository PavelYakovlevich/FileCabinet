using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    ///     Static class for holding all validation rules.
    /// </summary>
    public static class ValidationConstraints
    {
        /// <summary>
        ///     Default first name minimal length.
        /// </summary>
        public static readonly int DefaultFirstNameMinLength = 2;

        /// <summary>
        ///     Default first name maximum length.
        /// </summary>
        public static readonly int DefaultFirstNameMaxLength = 60;

        /// <summary>
        ///     Custom first name minimal length.
        /// </summary>
        public static readonly int CustomFirstNameMinLength = 2;

        /// <summary>
        ///     Custom first name maximum length.
        /// </summary>
        public static readonly int CustomFirstNameMaxLength = 60;

        /// <summary>
        ///     Default first name minimal length.
        /// </summary>
        public static readonly int DefaultLastNameMinLength = 2;

        /// <summary>
        ///     Default first name maximum length.
        /// </summary>
        public static readonly int DefaultLastNameMaxLength = 60;

        /// <summary>
        ///     Custom first name minimal length.
        /// </summary>
        public static readonly int CustomLastNameMinLength = 2;

        /// <summary>
        ///     Custom first name maximum length.
        /// </summary>
        public static readonly int CustomLastNameMaxLength = 60;

        /// <summary>
        ///     Default date of birth minimal value.
        /// </summary>
        public static readonly DateTime DefaultDateOfBirthMinValue = new DateTime(1950, 1, 1);

        /// <summary>
        ///     Default date of birth maximum value.
        /// </summary>
        public static readonly DateTime DefaultDateOfBirthMaxValue = DateTime.Now;

        /// <summary>
        ///     Custom date of birth minimal value.
        /// </summary>
        public static readonly DateTime CustomDateOfBirthMinValue = new DateTime(1950, 1, 1);

        /// <summary>
        ///     Custom date of birth maximum value.
        /// </summary>
        public static readonly DateTime CustomDateOfBirthMaxValue = DateTime.Now;

        /// <summary>
        ///     Default stature minimal value.
        /// </summary>
        public static readonly short DefaultStatureMinValue = 1;

        /// <summary>
        ///     Default stature maximum value.
        /// </summary>
        public static readonly short DefaultStatureMaxValue = short.MaxValue;

        /// <summary>
        ///     Custom stature minimal value.
        /// </summary>
        public static readonly short CustomStatureMinValue = 1;

        /// <summary>
        ///     Custom stature maximum value.
        /// </summary>
        public static readonly short CustomStatureMaxValue = 300;

        /// <summary>
        ///     Default weight minimal value.
        /// </summary>
        public static readonly decimal DefaultWeightMinValue = 1;

        /// <summary>
        ///     Default weight maximum value.
        /// </summary>
        public static readonly decimal DefaultWeightMaxValue = decimal.MaxValue;

        /// <summary>
        ///     Custom weight minimal value.
        /// </summary>
        public static readonly decimal CustomWeightMinValue = 1m;

        /// <summary>
        ///     Custom weight maximum value.
        /// </summary>
        public static readonly decimal CustomWeightMaxValue = 1000m;


        /// <summary>
        ///     Default gender allowed values.
        /// </summary>
        public static readonly char[] DefaultGenderValues = new[] { 'M', 'F' };

        /// <summary>
        ///     Custom gender allowed values.
        /// </summary>
        public static readonly char[] CustomGenderValues = new[] { 'M', 'F' };
    }
}
