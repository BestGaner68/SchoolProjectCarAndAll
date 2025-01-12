using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class WagenParkDataDto
    {
        public string AppUserId {get; set;}
        public int WagenparkId {get; set;}
        public int WagenparkVerzoekId {get; set;}
        public string VolledigeNaam {get; set;} = string.Empty;
        public string Email {get; set;} =string.Empty;   
    }
}