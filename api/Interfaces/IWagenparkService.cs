using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Verhuur;
using api.Dtos.WagenParkDtos;
using api.Models;

namespace api.Interfaces
{
    public interface IWagenparkService
    {
    /// <summary>
    /// methode maakt een wagenpark object aan in de database
    /// </summary>
    /// <param name="wagenpark">het wagenpark object</param>
    /// <param name="username">de gebruikersnaam van de beheerder</param>
    /// <returns>het aangemaakte wagenpark</returns>
    Task<WagenPark> CreateWagenparkAsync(WagenPark wagenpark, string username);

    /// <summary>
    /// methode returned het wagenpark van de beheerder gebaseerd op zijn appuserId
    /// </summary>
    /// <param name="AppUserId">de appuserId van de beheerder</param>
    /// <returns>het gevonden wagenpark</returns>
    Task<WagenPark?> GetBeheerdersWagenPark(string AppUserId);

    /// <summary>
    /// methode voor het maken van een verzoek om een wagenpark aan te maken voor bedrijven die een wagenpark willen openen 
    /// </summary>
    /// <param name="wagenParkVerzoekDto">het dto voor het nieuwe wagenparkverzoek met data over het wagenpark zoals bedrijfsnaam en kvk nummer, ook informatie over het account</param>
    Task NieuwWagenParkVerzoek(NieuwWagenParkVerzoekDto wagenParkVerzoekDto);

    /// <summary>
    /// methode voor het opvragen van de wagenparkverzoeken die nog moeten worden behandeld gebruikt in de frontend
    /// </summary>
    /// <returns>een lijst van alle wagenparkverzoeken</returns>
    Task<List<NieuwWagenParkVerzoek>> GetAllWagenparkVerzoekenAsync();

    /// <summary>
    /// methode voor het vinden van een wagenpark gebaseerd op wagenparkId
    /// </summary>
    /// <param name="WagenparkId">het id van het wagenpark</param>
    /// <returns>het gevonden wagenpark</returns>
    Task<WagenPark> GetWagenParkById(int WagenparkId);

    /// <summary>
    /// methode accepteert het verzoek van een wagenpark en maakt een nieuw wagenpark aan, gebruikt createwagenparkasync, maakt ook een gebruiker aan
    /// voor de wagenparkbeheerder en stuurd de informatie op via de email
    /// </summary>
    /// <param name="id">het id van het verzoek</param>
    /// <returns>het aangemaakte wagenpark</returns>
    Task<WagenPark> AcceptNieuwWagenParkVerzoek(int id);

    /// <summary>
    /// methode weigert het verzoek van een wagenpark en verstuurt een email met reden naar de gebruiker die een wagenpark wou aanmaken
    /// </summary>
    /// <param name="weigerNieuwWagenParkVerzoekDto">het dto met de weigering details en het verhuurverzoek id</param>
    /// <returns>true als de weigering succesvol was</returns>
    Task<bool> WeigerNieuwWagenParkVerzoek(WeigerNieuwWagenParkVerzoekDto weigerNieuwWagenParkVerzoekDto);

    /// <summary>
    /// methode checkt of de gebruiker al verwijderd is en dus geen toegang meer mag krijgen tot het wagenpark
    /// </summary>
    /// <param name="email">het emailadres van de gebruiker</param>
    /// <param name="wagenparkId">het id van het wagenpark</param>
    /// <returns>true als de gebruiker al verwijderd is</returns>
    Task<bool> IsVerwijderdeGebruiker(string email, int wagenparkId);

    }
}