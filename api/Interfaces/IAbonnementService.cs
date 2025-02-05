using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IAbonnementService
    {
        /// <summary>
        /// Methode zorgt voor het aanpassen van abonnementen in de database, de methode check het abonnement een payasyougo, waarna het direct het abonnement veranderd.
        /// als dit niet zo is zal het het nieuwe abonnement in de wachtrij zetten om in te veranderen nadat het momentele abonnement is afgelopen. Deze methode is specefiek voor 
        /// wagenparkbeheerders
        /// </summary>
        /// <param name="WagenParkId">het id van het wagenpark</param>
        /// <param name="nieuwAbonnementId">het id van het nieuw abonnement</param>
        /// <returns>niets</returns>
       Task<bool> WijzigAbonnementWagenpark (int WagenParkId, int nieuwAbonnementId);
       /// <summary>
       /// Methode zorgt voor het aanpassen van abonnementen in de database, de methode check het abonnement een payasyougo, waarna het direct het abonnement veranderd.
       /// als dit niet zo is zal het het nieuwe abonnement in de wachtrij zetten om in te veranderen nadat het momentele abonnement is afgelopen. Deze methode is specefiek voor 
       /// particuliere klanten
       /// </summary>
       /// <param name="appUserId">id van de particuliere klant</param>
       /// <param name="nieuweAbonnementId">id van het nieuwe abonnement</param>
       /// <returns></returns>
       Task <bool> WijzigAbonnementUser(string appUserId, int nieuweAbonnementId); 
       /// <summary>
       /// methode voor ophalen abonnementen voor klant uit db gebruik in frontend
       /// </summary>
       /// <returns>abonnementen met wagenparkabonnent == false</returns>
       Task<IEnumerable<Abonnement>> GetAllUserAbonnementen(); 
       /// <summary>
       /// methode voor het opvragen van het momenteel actieve abonnement van een wagenpark, kijkt in de db en checked of het een abonnent.isActief == true
       /// </summary>
       /// <param name="wagenparkId">id van het wagenpark</param>
       /// <returns>actieve abonnement van het wagenpark</returns>
       Task<Abonnement> GetActiveWagenparkAbonnement(int wagenparkId); 
       /// <summary>
       /// Methode voor het verlengen van het actieve wagenparkabonnent, het verlengt voor 3 maanden
       /// </summary>
       /// <param name="wagenparkId">id van het wagenpark waarvan het actieve abonnement moet worden verlengt</param>
       /// <returns>of het is gelukt</returns>
       Task<bool> ExtentCurrentAbonnement(int wagenparkId); 
       /// <summary>
       /// methode voor opvragen van actieve abonnement van de user, waar abonnent.isAcief == true
       /// </summary>
       /// <param name="appUserId"></param>
       /// <returns>het actieve abonnent van de user</returns>
       Task<Abonnement> GetUserAbonnement(string appUserId); 
       /// <summary>
       /// deze methode zorgt dat wanneer er een nieuwe user wordt aangemaakt dat ze gelijk een payasyou abonnent krijgen
       /// </summary>
       /// <param name="appUser">het appuser object dus het account van de gebruiker</param>
       /// <returns>of het is gelukt</returns>
       Task<bool> GeefStandaardAbonnement(AppUser appUser); 
       /// <summary>
       /// voegt een abonnent toe aan de abonnent tabel in de db, gebruik bij seeden db bij eerste opstart
       /// </summary>
       /// <param name="abonnements">lijst van de abonnent die moeten worden geinitialiseerd</param>
       /// <returns>niets</returns>
       Task AddAbonnement(List<Abonnement> abonnements); 
       /// <summary>
       /// returned alle abonnementen met wagenparkabonnent == true, gebruik in frontend
       /// </summary>
       /// <returns>de user abonnementen uit de db</returns>
       Task<IEnumerable<Abonnement>> GetAllWagenparkBeheerderAbonnementen(); 
       /// <summary>
       /// voegt een verzekering toe aan de db, gebruik bij seeden db
       /// Niets helemaal de juite interface
       /// </summary>
       /// <param name="verzekeringe">lijst van de verzekeringen</param>
       /// <returns>niets</returns>
       Task AddVerzekering(List<Verzekering> verzekeringe);
       /// <summary>
       /// voegt een accessoires toe aan de db, gebruik bij seeden db
       /// Niets helemaal de juite interface
       /// </summary>
       /// <param name="accessoires">lijst van de accessoiren</param>
       /// <returns>niets</returns>
       Task AddAccessoires(List<Accessoires> accessoires);
    }
}