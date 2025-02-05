using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.ReserveringenEnSchade
{
    public class WagenParkOverzichtDto
    {
        public DateTime StartDatum { get; set; }   
        public DateTime EindDatum { get; set; }
        public string AardReis  { get; set; }  =string.Empty; 
        public string Bestemming { get; set; } =string.Empty;
        public int VerwachtteKM { get; set; }
        public string VoertuigMerk { get; set; } =string.Empty;
        public string VoertuigType { get; set; } =string.Empty;
        public string VoertuigSoort { get; set; } =string.Empty;
        public string VolledigeNaam { get; set; } =string.Empty;
        public string Username { get; set; } =string.Empty;
        public string VoertuigStatus { get; set; } =string.Empty;
        public object ReserveringStatus { get;  set; } =string.Empty;
    }
}