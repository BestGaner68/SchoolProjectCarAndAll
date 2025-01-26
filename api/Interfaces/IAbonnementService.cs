using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IAbonnementService
    {
       Task<bool> WijzigAbonnementWagenpark (int WagenParkId, int nieuwAbonnementId);
       Task <bool> WijzigAbonnementUser(string appUserId, int nieuweAbonnementId);
       Task<IEnumerable<Abonnement>> GetAllUserAbonnementen();
       Task<Abonnement> GetActiveAbonnement(int wagenparkId);
       Task<bool> ExtentCurrentAbonnement(int wagenparkId);
       Task<Abonnement> GetUserAbonnement(string appUserId);
       Task<bool> GeefStandaardAbonnement(AppUser appUser);
       Task AddAbonnement(List<Abonnement> abonnements);
       Task<IEnumerable<Abonnement>> GetAllWagenparkBeheerderAbonnementen();
    }
}