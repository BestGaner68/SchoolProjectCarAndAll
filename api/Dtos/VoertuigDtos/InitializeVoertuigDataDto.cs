using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.VoertuigDtos
{
    public class InitializeVoertuigDataDto
    {
        public int VoertuigId { get; set; }
        public string Status { get; set; }
        public decimal KilometerPrijs { get; set; }
    }
}