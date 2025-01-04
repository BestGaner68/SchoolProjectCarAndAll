using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.ReserveringenEnSchade
{
    public class InnameDto
    {
        public bool isSchade;
        [Required]
        public string? Schade;
    }
}