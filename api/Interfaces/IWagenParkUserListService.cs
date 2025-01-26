using api.DataStructureClasses;
using api.Dtos.ReserveringenEnSchade;
using api.Models;

namespace api.Interfaces;

public interface IWagenParkUserListService
{
    Task<List<WagenParkOverzichtDto>> GetOverzicht(string wagenparkbeheerderId);
    Task <bool>StuurInvite(string email, string wagenparkbeheerderId);
    Task VerwijderGebruiker (string AppUserId);
    Task <List<AppUser>> GetAllUsersInWagenPark(string WagenParkBeheerderId);
    Task <WagenPark> GetWagenParkByAppUserEmail(string email);
    Task <WagenPark> GetWagenParkByAppUserId (string AppUserId);
    Task <bool> UpdateUserStatus(string AppUserId, string wagenParkUserListStatussen);
    Task<bool>PrimeUserInWagenParkUserList(string AppUserId, string NieuwStatus, string userEmail, int wagenparkId);
}