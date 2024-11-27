using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class VerhuurVerzoek
    {
        [Key]
        public int VerhuurVerzoekId { get; set; }
        public int VoertuigId { get; set; }
        public int KlantId { get; set; }
        public DateTime StartDatum { get; set; }   
        public DateTime EindDatum { get; set; }
        public string AardReis  { get; set; }  =string.Empty; 
        public string Bestemming { get; set; } =string.Empty;
        public int VerwachtteKM { get; set; }
        public DateTime Datum { get; set; } = DateTime.Now;
    }
}