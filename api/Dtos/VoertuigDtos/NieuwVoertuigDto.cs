using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.VoertuigDtos
{
    public class NieuwVoertuigDto
    { 
        [Required]
        public string Merk { get; set; } = string.Empty;
        [Required]
        public string Kenteken { get; set; } =string.Empty;
        [Required]
        public string Kleur { get; set; }  =string.Empty;
        [Required]
        public string Type {  get; set; } =string.Empty;
        [Required]
        public int AanschafJaar { get; set; }
        [Required]
        public string Soort {get; set;} = string.Empty;
    }
}