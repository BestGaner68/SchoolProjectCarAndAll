using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.KostenDtos;
using api.Models;

namespace api.Interfaces
{
    /// <summary>
    /// klasse is verantwoordelijk voor het berekenen van de kosten uit een reservering of een verhuurverzoek
    /// </summary>
    public interface IKostenService
    {
        /// <summary>
        /// na het online "innemen" van het voertuig kan deze methode een overzicht dto maken in de vorm (totale prijs : naam , kostenberekening)
        /// De functinaliteit komt uit de services in de api/Services/Kostenberekening
        /// voor meer informatie betreft deze services zie de klassen zelf
        /// Deze methode vraagt de services om verschillende kosten te berekenen en maakt een prijsoverzichtdto hiervan
        /// </summary>
        /// <param name="reserveringId">het id van de reservering</param>
        /// <param name="isSchade">of er schade is aan het voertuig</param>
        /// <param name="kilometersGereden">hoeveel kilometers er daadwerkelijk zijn gereden</param>
        /// <returns>een prijsoverzichtDto</returns>
        Task<PrijsOverzichtDto> BerekenTotalePrijs(int reserveringId, bool isSchade, decimal kilometersGereden);
        /// <summary>
        /// Berekent een verwachtte prijs van een verhuurverzoek voor overzicht voor de klant in de vorm (totale prijs : naam , kostenberekening)
        /// methode gebruikt voornamelijk services net als de vorige methode. Dit gebeurt door eerst te mappen naar een tijdelijke reservering, verspelt een overzicht van de kosten
        /// </summary>
        /// <param name="VerhuurverzoekId">id van het verhuurverzoek</param>
        /// <returns>een prijsoverzichtDtto</returns>
        Task<PrijsOverzichtDto> BerekenVerwachtePrijsUitVerhuurVerzoek(int VerhuurverzoekId);
    }
}