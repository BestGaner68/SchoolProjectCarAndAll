using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IKostenService
    {
        Task<decimal> BerekenVoorschot(int reserveringId, string appuserId);
        Task<decimal> BerekenDaadWerkelijkPrijs(int reserveringId, int KilometersGereden, bool isSchade, string appuserId);
        Task<decimal> BerekenVoorschotPrijsParticulier(int reserveringId);
        Task<decimal> BerekenDaadwerkelijkePrijsParticulier(int reserveringId, decimal kilometersDriven, bool isSchade);
        Task<decimal> BerekenVoorschotPrijsZakelijk(int reserveringId, Abonnement abonnement);
        Task<decimal> BerekenDaadwerkelijkePrijsZakelijk(int reserveringId, decimal kilometersDriven, bool isSchade, Abonnement abonnement);
    }
}