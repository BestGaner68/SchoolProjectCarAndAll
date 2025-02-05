using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Verhuur
{
    public class VolledigeDataDto
    {
        public DateTime StartDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public string AardReis { get; set; }
        public string Bestemming { get; set; }
        public int VerwachtteKM { get; set; }
        public string VolledigeNaam { get; set; }
        public string VoertuigMerk { get; set; }
        public string VoertuigSoort { get; set; }
        public string VoertuigType { get; set; }
        public int VerhuurverzoekId { get; set; }
        public List<string> Accessoires { get; set; } = new List<string>();
        public string Verzekering { get; set; }
    }   
}