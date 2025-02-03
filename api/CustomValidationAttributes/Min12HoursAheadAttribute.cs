using System;
using System.ComponentModel.DataAnnotations;

namespace api.CustomValidationAttributes
{
    /// <summary>
    /// constum attribute voor het valideren van de data, zodat dit minimaal 12 uur verschil heeft vanaf de datum nu
    /// </summary>
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