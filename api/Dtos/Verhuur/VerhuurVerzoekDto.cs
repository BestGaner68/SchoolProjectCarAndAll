using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Account
{
    public class VerhuurVerzoekDto
    {
        public DateTime StartDatum { get; set; }   
        public DateTime EindDatum { get; set; }
        public string AardReis  { get; set; }  =string.Empty; 
        public string Bestemming { get; set; } =string.Empty;
        public int VerwachtteKM { get; set; }
        public DateTime Datum { get; set; }
        public int VerhuurVerzoekId { get; set; }
    }
}
