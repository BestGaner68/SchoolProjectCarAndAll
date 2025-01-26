using System;
using System.ComponentModel.DataAnnotations;

namespace api.CustomValidationAttributes
{
    public class Min12HoursAheadAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                return date >= DateTime.Now.AddHours(12);
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} StartDatum moet minimaal 12 uur vanaf nu beginnen";
        }
    }
}