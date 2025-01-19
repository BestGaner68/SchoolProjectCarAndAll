using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IWagenparkService
    {
        Task<AppUser> GetUserId(string username);
        Task<WagenPark> CreateWagenparkAsync(WagenPark wagenpark, string username);
        Task<WagenPark> GetWagenParkByEmail(string email);
        Task<bool> CreateWagenParkVerzoek(string userId, int wagenparkId);
        Task<WagenPark?> GetBeheerdersWagenPark (string AppUserId);
        Task<WagenPark?> GetAppUsersWagenpark(string AppUserId);
        Task<List<AppUser>> GetAllUsers(string WagenparkBeheerderId);
    }
}