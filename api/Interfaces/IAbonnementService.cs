using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IAbonnementService
    {
       Task<bool> WijzigAbonnementWagenpark (int WagenParkId, int nieuwAbonnementId); //methode voor weizigen abonnement van een wagenpark
       Task <bool> WijzigAbonnementUser(string appUserId, int nieuweAbonnementId); //methode voor het wijzigen van abonnement van een user
       Task<IEnumerable<Abonnement>> GetAllUserAbonnementen(); //methode voor het opvragen van abonnementen die user kunnen kiezen
       Task<Abonnement> GetActiveWagenparkAbonnement(int wagenparkId); //methode voor het opvragen van het momenteel actieve abonnement van een wagenpark
       Task<bool> ExtentCurrentAbonnement(int wagenparkId); //methode voor het verlengen van een abonnement
       Task<Abonnement> GetUserAbonnement(string appUserId); //methode voor het opvragen van een abonnement van een gebruiker
       Task<bool> GeefStandaardAbonnement(AppUser appUser); //methode geeft een gebruiker het pay as you go abonnement, dit wordt gebruikt bij het aanmaken van een gebruiker
       Task AddAbonnement(List<Abonnement> abonnements); //methode is nodig voor het seeden van de applicatie bij eerste opstart.
       Task<IEnumerable<Abonnement>> GetAllWagenparkBeheerderAbonnementen(); //methode returned alle abonnementen die wagenparken kunnen kiezen
       Task AddVerzekering(List<Verzekering> verzekeringe);
       Task AddAccessoires(List<Accessoires> accessoires);
    }
}