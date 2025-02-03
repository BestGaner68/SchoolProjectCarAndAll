using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Reservering
    {
        [Key]
        public int ReserveringId { get; set; }
        public string AppUserId { get; set; }
        public int VoertuigId { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public string AardReis  { get; set; }  =string.Empty; 
        public string Bestemming { get; set; } =string.Empty;
        public int VerwachtteKM { get; set; }
        public string Fullname { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int VerzekeringId { get; set; }
        public Verzekering Verzekering { get; set; }
        public List<Accessoires>? Accessoires{ get; set; } = [];
    }
}