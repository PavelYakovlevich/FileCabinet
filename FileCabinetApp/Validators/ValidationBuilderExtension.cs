using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    public static class ValidationBuilderExtension
    {
        public static CompositeValidator CreateDefault(this ValidatorBuilder builder)
        {
            builder.ValidateFirstName(2, 60)
                .ValidateLastName(2, 60)
                .ValidateDateOfBirth(new DateTime(1950, 1, 1), DateTime.Now);
        }
    }
}
