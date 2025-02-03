using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.ReserveringenEnSchade;

namespace api.CustomValidationAttributes
{
    /// <summary>
    /// checked of er schade is ingevult als er aangegeven is dat er schade is
    /// </summary>
    public class RequiredIfSchadeAttribute : ValidationAttribute 
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