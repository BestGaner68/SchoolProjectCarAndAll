using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace api.Models
{
    /// <summary>
    /// basisdata van een voertuig wordt hier opgeslagen. heeft een voertuigdata waar nog extra informatie over het voertuig staat
    /// </summary>
    public class Voertuig
    {
        [Key]
        public int VoertuigId { get; set; } 
        public VoertuigData VoertuigData {get; set;} 
        public string Merk { get; set; } = string.Empty;
        public string Kenteken { get; set; } =string.Empty;
        public string Kleur { get; set; }  =string.Empty;
        public string Type {  get; set; } =string.Empty;
        public int AanschafJaar { get; set; }
        public string Soort {get; set;} = string.Empty; 
    }
}