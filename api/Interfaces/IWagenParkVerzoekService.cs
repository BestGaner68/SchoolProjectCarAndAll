using api.Dtos.ReserveringenEnSchade;
using api.Models;

namespace api.Interfaces;

public interface IWagenparkVerzoekService
{
    Task<bool> AcceptUserRequest (int verzoekId, string AppUserId);
    Task<bool> DenyUserRequest (int verzoekId, string AppUserId);
    Task<List<WagenParkVerzoek>> GetAllVerzoeken (string UserId);
    Task<bool> RemoveVerzoek (WagenParkVerzoek verzoek);
    Task<List<WagenParkOverzichtDto>> GetOverzicht(string wagenparkbeheerderId);
    Task<bool> RemoveUser(string AppUserId, string WagenParkOwnerId);
}