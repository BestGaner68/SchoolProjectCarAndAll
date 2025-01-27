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
        Task<PrijsOverzichtDto> BerekenVoorschot(int reserveringId, string appuserId);
        Task<PrijsOverzichtDto> BerekenDaadWerkelijkPrijs(int reserveringId, int KilometersGereden, bool isSchade, string appuserId);
        Task<PrijsOverzichtDto> BerekenVoorschotPrijsZakelijk(int reserveringId, Abonnement abonnement);
        Task<PrijsOverzichtDto> BerekenDaadwerkelijkePrijsZakelijk(int reserveringId, decimal kilometersDriven, bool isSchade, Abonnement abonnement);
        Task<PrijsOverzichtDto> BerekenVoorschotPrijsParticulier(int reserveringId, Abonnement abonnement);
        Task<PrijsOverzichtDto> BerekenDaadwerkelijkePrijsParticulier(int reserveringId, decimal kilometersDriven, bool isSchade, Abonnement abonnement);
        Task<PrijsOverzichtDto> BerekenPayAsYouGo(int reserveringId, bool IsSchade);
        Task<PrijsOverzichtDto> BerekenVerwachtePrijsUitVerhuurVerzoek(string appuserId, decimal kilometersDriven, DateTime startDatum, DateTime endDatum, int VoertuigId);
    }
}