using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.KostenDtos;
using api.Interfaces;
using api.Models;

namespace api.Service.KostenBerekeningen
{
    public class AccessoiresPrijsService
    {
        private readonly IReserveringService _reserveringRepo;
    
        public AccessoiresPrijsService(IReserveringService reserveringRepo)
        {
            _reserveringRepo = reserveringRepo;
        }
    
        public async Task<PrijsOverzichtDto> Bereken(int reserveringId)
        {
            var reservering = await _reserveringRepo.GetByIdOverzicht(reserveringId);
            decimal totalePrijs = reservering.Accessoires.Sum(a => a.Prijs);
    
            var prijsDetails = reservering.Accessoires
                .Select(a => new PrijsOnderdeelDto
                {
                    Beschrijving = $"Accessoire: {a.Naam}",
                    Amount = a.Prijs
                }).ToList();
    
            return new PrijsOverzichtDto { TotalePrijs = totalePrijs, PrijsDetails = prijsDetails };
        }

        public async Task<PrijsOverzichtDto> BerekenUitVerhuurVerzoek(Reservering reservering)
        {
            decimal totalePrijs = reservering.Accessoires.Sum(a => a.Prijs);
    
            var prijsDetails = reservering.Accessoires
                .Select(a => new PrijsOnderdeelDto
                {
                    Beschrijving = $"Accessoire: {a.Naam}",
                    Amount = a.Prijs
                }).ToList();
    
            return new PrijsOverzichtDto { TotalePrijs = totalePrijs, PrijsDetails = prijsDetails };
        }
    }
}