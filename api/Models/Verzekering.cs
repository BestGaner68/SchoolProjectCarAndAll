using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    /// <summary>
    /// structuur voor alle verzekeringen die we aanbieden. niets bijzonders
    /// </summary>
    public class Verzekering
    {
        [Key]
        public int VerzekeringId { get; set; }
        public string VerzekeringNaam { get; set; }
        public decimal VerzekeringPrijs {get; set;}
    }
}