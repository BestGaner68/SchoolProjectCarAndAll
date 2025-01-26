using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using api.DataStructureClasses;

namespace api.Models
{
    public class VoertuigData
    {
        [Key]
        public int VoertuigDataId{ get; set; }
        [ForeignKey("Voertuig")]
        public int VoertuigId { get; set; }
        [JsonIgnore]
        public Voertuig Voertuig { get; set; }
        public string Status { get; set; } = VoertuigStatussen.KlaarVoorGebruik;
        public string? Opmerking { get; set; }
        public decimal KilometerPrijs { get; set; }
    }
}