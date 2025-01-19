using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.CustomValidationAttributes;

namespace api.Dtos.ReserveringenEnSchade
{
    public class InnameDto
    {
        [Required]
        public int ReserveringId { get; set; }
        [Required]
        public bool IsSchade { get; set; }
        [RequiredIfSchade]
        public string? Schade {get; set;}
        public IFormFile? BeschrijvingFoto {get; set;}
    }
}