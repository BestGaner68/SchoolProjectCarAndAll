using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.Models
{
    public class WagenPark
    {
        [Key]
        public int WagenParkId { get; set; }
        public AppUser AppUser { get; set; }
        public string Bedrijfsnaam { get; set; } = string.Empty ;
        public string BedrijfsString { get; set; } = string.Empty;
        public string KvkNummer {get; set; } = string.Empty;
        public int MaxVoertuigen {get; set;} 
        public int VoertuigenInGebruik {get; set;}   
    }
}