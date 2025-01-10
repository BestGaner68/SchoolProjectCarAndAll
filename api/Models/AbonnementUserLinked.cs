using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class AbonnementUserLinked
    {
        [Key]
        public int AbonnementUserLinkedId { get; set; }
        public Abonnement Abonnement { get; set; }
        public int AbonnementId { get; set; }
        public AppUser WagenParkEigenaar {get; set; }
        public string AppUserId { get; set; }
    }
}