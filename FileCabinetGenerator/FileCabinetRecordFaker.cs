using System;
using System.Collections.Generic;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    /// <summary>
    ///     Class for generating a <see cref="FileCabinetRecord"/> object.
    /// </summary>
    public class FileCabinetRecordFaker
    {
        private static Dictionary<char, string[]> genderNamesDictionary = new Dictionary<char, string[]>()
        {
            { 'M', new[] { "Pavel", "Alexander", "Bob", "Phillip", "Josh", "Vitaliy", "Ivan", "Alexey", } },
            { 'F', new[] { "Maria", "Ulyana", "Victoria", "Alexandra", "Polina", "Anna", "Anastasia", "Sara", } },
        };

        private static string[] surnames = new[]
        {
            "Baker",
            "Johnson",
            "Green ",
            "Smith",
            "Jones",
            "Wilson",
            "Brown",
            "White",
        };

        private static char[] genders = new[]
        {
            'M',
            'F',
        };

        private int minYear;
        private Random random;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileCabinetRecordFaker"/> class.
        /// </summary>
        /// <param name="minYear">Minimal year of generated <see cref="DateTime"/> object.</param>
        public FileCabinetRecordFaker(int minYear)
        {
            this.minYear = minYear;
            this.random = new Random();
        }

        /// <summary>
        ///     Generates a <see cref="FileCabinetRecord"/> object.
        /// </summary>
        /// <param name="id">Id of generated <see cref="FileCabinetRecord"/> object.</param>
        /// <returns>Generated <see cref="FileCabinetRecord"/> object.</returns>
        public FileCabinetRecord Generate(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException($"{nameof(id)}", nameof(id));
            }

            var result = new FileCabinetRecord();

            result.Id = id;
            result.Gender = this.TakeRandomItem(genders);
            result.FirstName = this.TakeRandomItem(genderNamesDictionary[result.Gender]);
            result.LastName = this.TakeRandomItem(surnames);
            result.DateOfBirth = this.GenerateDateTime();
            result.Stature = this.GenerateStature();
            result.Weight = this.GenerateWeight();

            return result;
        }

        private decimal GenerateWeight()
        {
            return Math.Round((decimal)this.random.NextDouble() * 100, 2);
        }

        private short GenerateStature()
        {
            return (short)this.random.Next(160, 300);
        }

        private DateTime GenerateDateTime()
        {
            int year = this.random.Next(this.minYear, DateTime.Now.Year);
            int month = this.random.Next(1, 12);
            int day = this.random.Next(1, DateTime.DaysInMonth(year, month));

            return new DateTime(year, month, day);
        }

        private T TakeRandomItem<T>(T[] values)
        {
            var index = this.random.Next(0, values.Length);
            return values[index];
        }
    }
}
