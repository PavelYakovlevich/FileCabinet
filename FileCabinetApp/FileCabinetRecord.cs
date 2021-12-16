using System;

namespace FileCabinetApp
{
    public class FileCabinetRecord
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public short Stature { get; set; }

        public char Gender { get; set; }

        public decimal Weight { get; set; }

        public override string ToString()
        {
            return $"{this.Id}, {this.FirstName}, {this.LastName}, {this.DateOfBirth.ToString("yyyy-MMM-dd")}, {this.Gender}, {this.Stature}, {this.Weight}";
        }
    }
}
