using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Verhuur
{
    public class WeigerVerhuurVerzoekDto
    {
        public int VerhuurverzoekId { get; set; }
        public string RedenVoorWeigering {get; set;}
    }
}