using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.ReserveringenEnSchade;

namespace api.CustomValidationAttributes
{
    public class RequiredIfSchadeAttribute : ValidationAttribute //custom attribute voor valideren van dto
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var dto = (InnameDto)validationContext.ObjectInstance;
    
            if (dto.IsSchade && string.IsNullOrWhiteSpace(value as string))
            {
                return new ValidationResult("Schade is verplicht wanneer er schade is gemeld.");
            }
    
            return ValidationResult.Success;
        }

    }
}