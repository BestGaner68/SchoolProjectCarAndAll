using api.Migrations;
using api.Models;

namespace api.Interfaces;

public interface IWagenparkVerzoekService
{
    Task<bool> AcceptUserRequest (int verzoekId);
    Task<bool> DenyUserRequest (int verzoekId);
    Task<List<AppUser>> GetAllUsers (String WagenparkBeheerderId);
    Task<List<WagenParkVerzoek>> GetAllVerzoeken (string UserId);
    Task<bool> RemoveVerzoek (WagenParkVerzoek verzoek);
}