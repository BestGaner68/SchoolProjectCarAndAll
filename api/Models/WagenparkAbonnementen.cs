using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace api.Models
{
    public class WagenparkAbonnementen
    {
        [Key]
        public int WagenparkAbonnementId { get; set; }
        public int WagenParkId { get; set; }
        [JsonIgnore]
        public WagenPark WagenPark { get; set; }
        public int AbonnementId { get; set; }
        public Abonnement Abonnement { get; set; } 
        public DateTime StartDatum { get; set; }
        public DateTime? EindDatum { get; set; }
        public bool IsActief { get; set; }
        public bool IsVolgendAbonnement { get; set; }
    }
}