using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    /// <summary>
    /// niets interessant hier, alleen data die nodig is bij het aanmaken van een wagenpark
    /// </summary>
    public class NieuwWagenParkVerzoek
    {
        [Key]
        public int WagenparkVerzoekId { get; set; } 
        public string Voornaam { get; set; } = string.Empty;
        public string Achternaam { get; set; } = string.Empty;
        public string GewensdeUsername { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Bedrijfsnaam { get; set; } = string.Empty;
        public string KvkNummer { get; set; } = string.Empty;
        
    }
}