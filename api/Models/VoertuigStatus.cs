using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.DataStructureClasses;

namespace api.Models
{
    public class VoertuigStatus
    {
        [Key]
        public int VoertuigId { get; set; }
        public string Status { get; set; } = VoertuigStatussen.KlaarVoorGebruik;
        public string? Opmerking { get; set; }
    }
}