using api.DataStructureClasses;
using api.Dtos.ReserveringenEnSchade;
using api.Models;
using Microsoft.OpenApi.Expressions;

namespace api.Interfaces;

public interface IWagenParkUserListService
{
    /// <summary>
    /// methode returned een overzicht van alle reserveringen die gebruikers in het wagenpark hebben gedaan. in de vorm van een overzicht met extra informatie
    /// </summary>
    /// <param name="wagenparkbeheerderId">de appuserid van de wagenparkbeheerder</param>
    /// <returns>Uitgebereide informatie van de reservering, voertuignaam, merk, volledige naam gebruiker</returns>
    Task<List<WagenParkOverzichtDto>> GetOverzicht(string wagenparkbeheerderId);

    /// <summary>
    /// methode voor het sturen van een invite naar een gebruiker om het wagenpark te joinen, gebruiker kan hierna een bedrijfsaccount aanmaken
    /// </summary>
    /// <param name="email">het emailadres van de gebruiker</param>
    /// <param name="wagenparkbeheerderId">de id van de wagenparkbeheerder</param>
    /// <returns>true als de invite succesvol is verzonden</returns>
    Task<bool> StuurInvite(string email, string wagenparkbeheerderId);

    /// <summary>
    /// methode voor het permanent verwijderen van het account van een gebruiker, gebruiker kan niet meer een nieuw zakelijk account aanmaken met hetzelfde email
    /// </summary>
    /// <param name="AppUserId">de id van de gebruiker</param>
    /// <returns>true als de gebruiker succesvol is verwijderd</returns>
    Task<bool> VerwijderGebruiker(string AppUserId);

    /// <summary>
    /// methode returned alle gebruikers in het wagenpark van de beheerder, alleen de beheerder kan dit doen
    /// </summary>
    /// <param name="WagenParkBeheerderId">de id van de wagenparkbeheerder</param>
    /// <returns>een lijst met alle gebruikers in het wagenpark</returns>
    Task<List<AppUser>> GetAllUsersInWagenPark(string WagenParkBeheerderId);

    /// <summary>
    /// methode returned het wagenpark gebaseerd op de email van de gebruiker, niche gebruik
    /// </summary>
    /// <param name="email">het emailadres van de gebruiker</param>
    /// <returns>het gevonden wagenpark</returns>
    Task<WagenPark> GetWagenParkByAppUserEmail(string email);

    /// <summary>
    /// methode returned het wagenpark object gebaseerd op appuserId
    /// </summary>
    /// <param name="AppUserId">het id van de gebruiker</param>
    /// <returns>het gevonden wagenpark</returns>
    Task<WagenPark> GetWagenParkByAppUserId(string AppUserId);

    /// <summary>
    /// methode voor het aanpassen en bijhouden van de status van een gebruiker, zodat er overzicht gehouden kan worden in de database
    /// </summary>
    /// <param name="AppUserId">de id van de gebruiker</param>
    /// <param name="wagenParkUserListStatussen">de nieuwe status van de gebruiker</param>
    /// <returns>true als de status succesvol is bijgewerkt</returns>
    Task<bool> UpdateUserStatus(string AppUserId, string wagenParkUserListStatussen);

    /// <summary>
    /// methode wordt gebruikt om alle data te initialiseren bij het aanmaken van een zakelijk account, zodat alles goed verwerkt is in de database
    /// gebruik bij aanmaken user
    /// </summary>
    /// <param name="AppUserId">de id van de gebruiker</param>
    /// <param name="NieuwStatus">de nieuwe status van de gebruiker</param>
    /// <param name="userEmail">het emailadres van de gebruiker</param>
    /// <param name="wagenparkId">de id van het wagenpark</param>
    /// <returns>true als de data succesvol is geïnitialiseerd</returns>
    Task<bool> PrimeUserInWagenParkUserList(string AppUserId, string NieuwStatus, string userEmail, int wagenparkId);

}