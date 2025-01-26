using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.KostenDtos
{
    public class PrijsOverzichtDto
    {
        public decimal TotalePrijs {get; set;}
        public List<PrijsOnderdeelDto> PrijsDetails {get ; set;}
    }
}