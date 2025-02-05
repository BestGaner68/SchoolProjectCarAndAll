using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dtos.ReserveringenEnSchade
{
    public class HuurGeschiedenisDto
    {
        public DateTime StartDatum { get; set; }   
        public DateTime EindDatum { get; set; }
        public string AardReis  { get; set; } = string.Empty; 
        public string Bestemming { get; set; } = string.Empty;
        public int VerwachtteKM { get; set; }
        public string VoertuigMerk { get; set; } = string.Empty;
        public string VoertuigType { get; set; } = string.Empty;
        public string VoertuigSoort { get; set; } = string.Empty;
        public string Verzekering { get; set; } = string.Empty;
        public List<Accessoires?> Accessoires { get; set; } = [];
    }
}