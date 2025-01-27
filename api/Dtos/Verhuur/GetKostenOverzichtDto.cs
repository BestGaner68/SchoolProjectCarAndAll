using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Verhuur
{
    public class GetKostenOverzichtDto
    {
        public int VoertuigId { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public decimal VerwachtteKM { get; set; }
    }
}