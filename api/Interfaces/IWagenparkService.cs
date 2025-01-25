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
        Task<AppUser> GetUserId(string username);
        Task<WagenPark> CreateWagenparkAsync(WagenPark wagenpark, string username);
        Task<WagenPark?> GetBeheerdersWagenPark (string AppUserId);
        Task NieuwWagenParkVerzoek(NieuwWagenParkVerzoekDto wagenParkVerzoekDto);
        Task<List<NieuwWagenParkVerzoek>> GetAllWagenparkVerzoekenAsync();
        Task<WagenPark> GetWagenParkById(int WagenparkId);
        Task<WagenPark> AcceptNieuwWagenParkVerzoek(int id);
        Task<bool> WeigerNieuwWagenParkVerzoek(WeigerNieuwWagenParkVerzoekDto weigerNieuwWagenParkVerzoekDto);
    }
}