using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.WagenParkDtos
{
    public class NodigUitDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}