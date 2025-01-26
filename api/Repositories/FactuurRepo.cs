using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.KostenDtos;
using api.Interfaces;
using api.Models;

namespace api.Repositories
{
    public class FactuurRepo : IFactuurService
    {
        public async Task<Factuur> MaakFactuur(Reservering reservering, PrijsOverzichtDto prijsOverzicht, AppUser appUser)
        {
            var factuur = new Factuur
            {
                Factuurnummer = "INV" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                KlantNaam = $"{appUser.Voornaam} {appUser.Achternaam}",
                KlantEmail = appUser.Email,
                Bedrag = prijsOverzicht.TotalePrijs,
                Datum = DateTime.Now,
                PrijsDetails = prijsOverzicht.PrijsDetails.Select(detail => new PrijsOnderdeelDto
                {
                    Beschrijving = detail.Beschrijving,
                    Amount = detail.Amount
                }).ToList()
            };
            return factuur;
        }
    }   
}  