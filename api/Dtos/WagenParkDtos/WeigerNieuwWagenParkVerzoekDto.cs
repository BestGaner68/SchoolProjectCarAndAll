using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.WagenParkDtos
{
    public class WeigerNieuwWagenParkVerzoekDto
    {
        public int WagenParkId;
        public string? Reden {get; set;}
    }
}