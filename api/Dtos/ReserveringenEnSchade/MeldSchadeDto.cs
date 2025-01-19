using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.ReserveringenEnSchade
{
    public class MeldSchadeDto
    {
        public int VoertuigId {get; set;}
        public string Schade {get; set;} = string.Empty;
        public IFormFile? SchadeFoto {get; set;}
    }
}