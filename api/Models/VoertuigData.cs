using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.DataStructureClasses;

namespace api.Models
{
    public class VoertuigData
    {
        [Key]
        public int VoertuigId { get; set; }
        public Voertuig Voertuig { get; set; }
        public string Status { get; set; } = VoertuigStatussen.KlaarVoorGebruik;
        public string? Opmerking { get; set; }
        public decimal KilometerPrijs { get; set; }
    }
}