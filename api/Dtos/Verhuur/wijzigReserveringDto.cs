using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Verhuur
{
    public class WijzigReserveringDto
    {
        public int ReserveringId { get; set; }
        public DateTime? NewStartDatum { get; set; }
        public DateTime? NewEindDatum { get; set; }
        public int? NieuwVoertuigId { get; set; }
    }
}