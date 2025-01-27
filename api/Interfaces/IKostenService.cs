using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.KostenDtos;
using api.Models;

namespace api.Interfaces
{
    public interface IKostenService
    {
        Task<PrijsOverzichtDto> BerekenVoorschot(int reserveringId, string appuserId); //methode voor het berekenen van het voorschot dat de gebruiker moet afleveren of voorschieten voor de reservering
        Task<PrijsOverzichtDto> BerekenDaadWerkelijkPrijs(int reserveringId, int KilometersGereden, bool isSchade, string appuserId); //methode berekend de daadwerkelijk kosten van een reservering gebaseerd op de ingevulde gegevens
        Task<PrijsOverzichtDto> BerekenVerwachtePrijsUitVerhuurVerzoek(string appuserId, decimal kilometersDriven, DateTime startDatum, DateTime endDatum, int VoertuigId); //berekend de verwachtte kosten en stuur dit naar de frontend zodat de gebruiker kan zien wat de 
        //verwachtte kosten van de reservering zullen zijn
        //De rest van de methodes zijn private en zijn verschillende implementaties van het berekenen van de kosten, de bovenstaande methodes sturen de gebruiker naar de juiste submethode voor het 
        //bereken van de juiste kosten gebaseerd op het type gebruiker en wat voor abonnement zij hebben
    }
}