using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace api.Models
{
    public class VerhuurVerzoek
    {
        [Key]
        public int VerhuurVerzoekId { get; set; }
        public int VoertuigId { get; set; }
        public string AppUserId { get; set; } = string.Empty;   
        public DateTime StartDatum { get; set; }   
        public DateTime EindDatum { get; set; }
        public string AardReis  { get; set; }  =string.Empty; 
        public string Bestemming { get; set; } =string.Empty;
        public int VerwachtteKM { get; set; }
        public DateTime Datum { get; set; } = DateTime.Now;
        public string Status {get; set;} =string.Empty;
        public int VerzekeringId { get; set; }
        [ForeignKey("VerzekeringId")]
        public Verzekering Verzekering { get; set; }
        public List<Accessoires>? Accessoires{ get; set; } =[];
    }
}