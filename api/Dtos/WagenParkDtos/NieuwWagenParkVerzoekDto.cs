using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.WagenParkDtos
{
    public class NieuwWagenParkVerzoekDto
    {
        [Required(ErrorMessage = "Voornaam is verplicht.")]
        [StringLength(100, ErrorMessage = "Voornaam mag maximaal 100 tekens bevatten.")]
        public string Voornaam { get; set; } = string.Empty;

        [Required(ErrorMessage = "Achternaam is verplicht.")]
        [StringLength(100, ErrorMessage = "Achternaam mag maximaal 100 tekens bevatten.")]
        public string Achternaam { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gewensde username is verplicht.")]
        [StringLength(50, ErrorMessage = "Gewensde username mag maximaal 50 tekens bevatten.")]
        public string GewensdeUsername { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is verplicht.")]
        [EmailAddress(ErrorMessage = "Voer een geldig e-mailadres in.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bedrijfsnaam is verplicht.")]
        [StringLength(150, ErrorMessage = "Bedrijfsnaam mag maximaal 150 tekens bevatten.")]
        public string Bedrijfsnaam { get; set; } = string.Empty;

        [Required(ErrorMessage = "KvkNummer is verplicht.")]
        [StringLength(10, ErrorMessage = "KvkNummer moet maximaal 10 tekens bevatten.")]
        public string KvkNummer { get; set; } = string.Empty;
    }
}