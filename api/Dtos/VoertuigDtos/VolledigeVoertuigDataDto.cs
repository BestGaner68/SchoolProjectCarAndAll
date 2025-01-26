using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dtos.VoertuigDtos
{
    public class VolledigeVoertuigDataDto
    {
        public Voertuig Voertuig{ get; set; }
        public List<SchadeFormulier>? Schades {get; set;}
    }
}