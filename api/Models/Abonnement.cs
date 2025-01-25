using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Abonnement
    {
        [Key]
        public int AbonnementId { get; set; }
        public string Naam { get; set; } = string.Empty;
        public decimal Prijs { get; set; }
        public bool IsStandaard { get; set; }
        public bool IsWagenparkAbonnement { get; set; }
        public ICollection<UserAbonnement> UserAbonnementen { get; set; } 
    }
}