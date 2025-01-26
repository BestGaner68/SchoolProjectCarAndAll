using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.VoertuigDtos
{
    public class SchadeMeldingBehandelDto
    {
        [Required]
        public int SchadeFormulierId {get; set;}
        [Required]
        public string ReparatieOpmerking {get; set;}= string.Empty;
    }
}