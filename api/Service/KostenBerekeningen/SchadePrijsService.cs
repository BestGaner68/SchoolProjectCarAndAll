using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DataStructureClasses;
using api.Dtos.KostenDtos;
using api.Models;

namespace api.Service.KostenBerekeningen
{
    public class SchadePrijsService
    {
        public PrijsOverzichtDto Bereken(bool isSchade, Verzekering verzekering)
        {
            decimal standaardSchadePrijs = isSchade ? 100m : 0m;  
            decimal korting = verzekering.VerzekeringNaam switch
            {
                VerzekeringNamen.GeenVerzekering => 0m, 
                VerzekeringNamen.HalfVerzekering => 0.5m, 
                VerzekeringNamen.VolVezekering => 1m, 
                _ => throw new InvalidOperationException($"Onbekende verzekering: {verzekering.VerzekeringNaam}")
            };
    
            decimal aangepasteSchadePrijs = standaardSchadePrijs * (1 - korting);
    
            var prijsDetails = new List<PrijsOnderdeelDto>
            {
                new() { Beschrijving = "Standaard schadeprijs", Amount = standaardSchadePrijs },
                new() { Beschrijving = $"Verzekeringskorting ({verzekering.VerzekeringNaam})", Amount = -(standaardSchadePrijs * korting) }
            };
    
            return new PrijsOverzichtDto
            {
                TotalePrijs = aangepasteSchadePrijs,
                PrijsDetails = prijsDetails
            };
        }
    }

}