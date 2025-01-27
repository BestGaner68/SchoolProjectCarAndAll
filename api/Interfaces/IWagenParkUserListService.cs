using api.DataStructureClasses;
using api.Dtos.ReserveringenEnSchade;
using api.Models;
using Microsoft.OpenApi.Expressions;

namespace api.Interfaces;

public interface IWagenParkUserListService
{
    Task<List<WagenParkOverzichtDto>> GetOverzicht(string wagenparkbeheerderId); //Methode returned een overzicht van alle gebruikers reserveringen
    Task <bool>StuurInvite(string email, string wagenparkbeheerderId); //Methode voor het sturen van een invite naar een gebruiker om het wagenpark te joinen
    Task <bool> VerwijderGebruiker (string AppUserId); //Methode voor het verwijderen van het account van een gebruiker PERMANENET
    Task <List<AppUser>> GetAllUsersInWagenPark(string WagenParkBeheerderId); //Methode returned alle gebruikers in de beheerder wagenpark
    Task <WagenPark> GetWagenParkByAppUserEmail(string email); //Methode returned het wagenpark gebaseerd op de email van de gebruiker, Niche gebruik
    Task <WagenPark> GetWagenParkByAppUserId (string AppUserId); //Methode returned het wagenpark gebaseerd op appuserId
    Task <bool> UpdateUserStatus(string AppUserId, string wagenParkUserListStatussen); //Methode voor het aanpassen en bijhouden van de status van een gebruiker
    Task<bool>PrimeUserInWagenParkUserList(string AppUserId, string NieuwStatus, string userEmail, int wagenparkId); //Methode wordt gebruikt om alle data te initailiseren bij het aanmaken van een zakelijk account
}