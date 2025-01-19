using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class AbonnementWagenparkLinked
    {
        [Key]
        public int AbonnementWagenparkLinkedId { get; set; }
        public Abonnement Abonnement { get; set; }
        public int AbonnementId { get; set; }
        public WagenPark wagenPark{ get; set; }
        public int WagenParkId { get; set;}
        public DateTime StartDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public int GeredenKilometers {get; set;}
    }
}