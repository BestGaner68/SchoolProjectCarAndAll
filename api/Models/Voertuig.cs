using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace api.Models
{
    public class Voertuig
    {
        [Key]
        public int VoertuigId { get; set; }  
        public string Merk { get; set; } = string.Empty;
        public string Kenteken { get; set; } =string.Empty;
        public string Kleur { get; set; }  =string.Empty;
        public string Type {  get; set; } =string.Empty;
        public int AanschafJaar { get; set; }
        public string Soort {get; set;} = string.Empty;
        public int KilometerPrijs {get; set;}   
    }
}