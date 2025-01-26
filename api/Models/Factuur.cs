using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.KostenDtos;

namespace api.Models
{
    public class Factuur
    {
        public string Factuurnummer { get; set; }
        public string KlantNaam { get; set; }
        public string KlantEmail { get; set; }
        public decimal Bedrag { get; set; }
        public DateTime Datum { get; set; }
        public List<PrijsOnderdeelDto> PrijsDetails { get; set; }
    }
}