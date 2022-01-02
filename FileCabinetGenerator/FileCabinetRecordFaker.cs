using System;
using System.Collections.Generic;
using FileCabinetApp;

namespace FileCabinetGenerator
{
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

        public FileCabinetRecordFaker(int minYear)
        {
            this.minYear = minYear;
        }

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
            return Math.Round((decimal)new Random().NextDouble() * 100, 2);
        }

        private short GenerateStature()
        {
            return (short)new Random().Next(160, 300);
        }

        private DateTime GenerateDateTime()
        {
            var random = new Random();

            int year = random.Next(this.minYear, DateTime.Now.Year);
            int month = random.Next(1, 12);
            int day = random.Next(1, DateTime.DaysInMonth(year, month));

            return new DateTime(year, month, day);
        }

        private T TakeRandomItem<T>(T[] values)
        {
            var random = new Random();
            var index = random.Next(0, values.Length - 1);
            return values[index];
        }
    }
}
