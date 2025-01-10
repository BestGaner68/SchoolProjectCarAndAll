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
        public string AbonnementType { get; set; } = string.Empty;
        public int AbonnementPrijs {get; set;}
    }
}