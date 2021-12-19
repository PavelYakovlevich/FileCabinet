using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Guard guard = new Guard();

        public FileCabinetService()
        {
#if DEBUG

            this.list.Add(new FileCabinetRecord
            {
                Id = 1,
                FirstName = "Pavel",
                LastName = "Yakovlevich",
                DateOfBirth = new DateTime(2000, 7, 14),
                Stature = 180,
                Weight = 82.01m,
                Gender = 'M',
            });

            this.list.Add(new FileCabinetRecord
            {
                Id = 2,
                FirstName = "Petr",
                LastName = "Semenov",
                DateOfBirth = new DateTime(1994, 12, 10),
                Stature = 160,
                Weight = 100.54m,
                Gender = 'M',
            });

            this.list.Add(new FileCabinetRecord
            {
                Id = 3,
                FirstName = "Vasil",
                LastName = "Semenov",
                DateOfBirth = new DateTime(1999, 02, 15),
                Stature = 160,
                Weight = 100.54m,
                Gender = 'M',
            });
#endif
        }

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, char gender, decimal weight, short stature)
        {
            this.CheckRequirements(firstName, lastName, dateOfBirth, gender, weight, stature);

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

        public int GetStat()
        {
            return this.list.Count;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, char gender, decimal weight, short stature)
        {
            this.CheckRequirements(firstName, lastName, dateOfBirth, gender, weight, stature);

            var targetRecord = this.GetRecord(id);

            this.guard.Requires(() => targetRecord is not null, $"User with {id} does not exist!");

            targetRecord!.FirstName = firstName;
            targetRecord!.LastName = lastName;
            targetRecord!.DateOfBirth = dateOfBirth;
            targetRecord!.Gender = gender;
            targetRecord!.Weight = weight;
            targetRecord!.Stature = stature;
        }

        public FileCabinetRecord? GetRecord(int id)
        {
            foreach (var record in this.list)
            {
                if (record.Id == id)
                {
                    return record;
                }
            }

            return null;
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            return this.FindByCondition(rec => rec?.FirstName?.Equals(firstName) ?? false);
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            return this.FindByCondition(rec => rec?.LastName?.Equals(lastName) ?? false);
        }

        private FileCabinetRecord[] FindByCondition(Predicate<FileCabinetRecord> condition)
        {
            var result = new List<FileCabinetRecord>();

            foreach (var record in this.list)
            {
                if (condition(record))
                {
                    result.Add(record);
                }
            }

            return result.ToArray();
        }

        private void CheckRequirements(string firstName, string lastName, DateTime dateOfBirth, char gender, decimal weight, short stature)
        {
            this.guard.IsNotNull(firstName, nameof(firstName))
                .IsNotEmptyOrWhiteSpace(firstName, nameof(firstName))
                .Requires(() => firstName.Length >= 2 && firstName.Length <= 60, $"{nameof(firstName)} lenght must be greate than 1 and less than 61.");

            this.guard.IsNotNull(lastName, nameof(lastName))
                .IsNotEmptyOrWhiteSpace(lastName, nameof(lastName))
                .Requires(() => lastName.Length >= 2 && lastName.Length <= 60, $"{nameof(lastName)} lenght must be greate than 1 and less than 61.");

            this.guard.Requires(
                () => { return dateOfBirth.CompareTo(new DateTime(1950, 1, 1)) >= 0 && dateOfBirth.CompareTo(DateTime.Now) <= 0; },
                $"{nameof(dateOfBirth)} must be greater than 01-Jan-1950 and less or equal to current date.");

            this.guard.IsInRange(gender, new[] { 'M', 'F' });

            this.guard.GreaterThan(weight, 0);

            this.guard.GreaterThan(stature, 0);
        }
    }
}
