using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Voertuig
{
    public class InitializeVoertuigDto
    {
        public string Merk { get; set; }
        public string Kenteken { get; set; }
        public string Kleur { get; set; }
        public string Type { get; set; }
        public int AanschafJaar { get; set; }
        public string Soort { get; set; }
    }
}