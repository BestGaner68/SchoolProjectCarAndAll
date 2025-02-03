using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.CustomValidationAttributes;

namespace api.Dtos.Account
{
    public class VerhuurVerzoekRequestDto
    {
        [Required(ErrorMessage = "VoertuigId is verplicht.")]
        public int VoertuigId { get; set; }
    
        [Required(ErrorMessage = "StartDatum is verplicht.")]
        [Min12HoursAhead]
        public DateTime StartDatum { get; set; }

        [Required(ErrorMessage = "EindDatum is verplicht.")]
        public DateTime EindDatum { get; set; }
    
        [Required(ErrorMessage = "AardReis is verplicht.")]
        [StringLength(100, ErrorMessage = "AardReis mag maximaal 100 tekens bevatten.")]
        public string AardReis { get; set; } = string.Empty;
    
        [Required(ErrorMessage = "Bestemming is verplicht.")]
        [StringLength(200, ErrorMessage = "Bestemming mag maximaal 200 tekens bevatten.")]
        public string Bestemming { get; set; } = string.Empty;
    
        [Required(ErrorMessage = "VerwachtteKM is verplicht.")]
        [Range(0, int.MaxValue, ErrorMessage = "VerwachtteKM moet een positief getal zijn.")]
        public int VerwachtteKM { get; set; }
        public List<int?> AccessoiresIds { get; set; }
        [Required(ErrorMessage = "Verzekering is verplicht.")]
        [Range(1, int.MaxValue, ErrorMessage = "Ongeldige verzekering geselecteerd.")]
        public int VerzekeringId { get; set; }
    }
}