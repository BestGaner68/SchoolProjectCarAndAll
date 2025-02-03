using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Accessoires
    {
        [Key]
        public int AccessoiresId { get; set; }
        public string Naam {get; set;}
        public decimal Prijs {get; set;}        
    }
}