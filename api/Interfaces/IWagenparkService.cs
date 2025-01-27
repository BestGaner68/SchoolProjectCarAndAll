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
        Task<AppUser> GetUserId(string username); //methode zoek de juiste appuserId bij de username
        Task<WagenPark> CreateWagenparkAsync(WagenPark wagenpark, string username); //methode maakt een wagenpark aan
        Task<WagenPark?> GetBeheerdersWagenPark (string AppUserId); //methode returned het wagenpark van de beheerder gebaseerd op zijn appuserId
        Task NieuwWagenParkVerzoek(NieuwWagenParkVerzoekDto wagenParkVerzoekDto);//methode voor het maken van een verzoek om een wagenpark aan te maken
        Task<List<NieuwWagenParkVerzoek>> GetAllWagenparkVerzoekenAsync(); //methode voor het opvragen van de wagenparkverzoeken gebruk in frontend
        Task<WagenPark> GetWagenParkById(int WagenparkId); //methode voor het vinden van een wagenpark gebaseerd op wagenparkID
        Task<WagenPark> AcceptNieuwWagenParkVerzoek(int id); //methode accepteerd het verzoek van een wagenpark en maakt een nieuw wagenpark aan, gebruik createwagenparkasync
        Task<bool> WeigerNieuwWagenParkVerzoek(WeigerNieuwWagenParkVerzoekDto weigerNieuwWagenParkVerzoekDto); // declined en verstuurd email met reden naar de gebruiker die een wagenpark wou aanmaken
        Task <bool> IsVerwijderdeGebruiker(string email, int wagenparkId); //methode check of de gebruiker al verwijderd is en dus geen toegang meer mag krijgen tot het wagenpark
    }
}