using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, char gender, decimal weight, short stature)
        {
            var guard = new Guard();

            guard.IsNotNull(firstName, nameof(firstName))
                .IsNotEmptyOrWhiteSpace(firstName, nameof(firstName))
                .Requires(() => { return firstName.Length >= 2 && firstName.Length <= 60; }, $"{nameof(firstName)} lenght must be greate than 1 and less than 61.");

            guard.IsNotNull(lastName, nameof(lastName))
                .IsNotEmptyOrWhiteSpace(lastName, nameof(lastName))
                .Requires(() => { return lastName.Length >= 2 && lastName.Length <= 60; }, $"{nameof(lastName)} lenght must be greate than 1 and less than 61.");

            guard.Requires(
                () => { return dateOfBirth.CompareTo(new DateTime(1950, 1, 1)) >= 0 && dateOfBirth.CompareTo(DateTime.Now) <= 0; },
                $"{nameof(dateOfBirth)} must be greater than 01-Jan-1950 and less or equal to current date.");

            guard.IsInRange(gender, new[] { 'M', 'F' });

            guard.GreaterThan(weight, 0);

            guard.GreaterThan(stature, 0);

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Gender = gender,
                Weight = weight,
                Stature = stature,
            };

            this.list.Add(record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }
    }
}
