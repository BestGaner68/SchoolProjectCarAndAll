using api.Migrations;
using api.Models;

namespace api.Interfaces;

public interface IWagenparkVerzoekService
{
    Task<bool> AcceptUserRequest (int verzoekId);
    Task<bool> DenyUserRequest (int verzoekId);
    Task<List<AppUser>> GetAllUsers (int id);
    Task<List<WagenParkVerzoek>> GetAllVerzoeken (int wagenparkId);
    Task<bool> RemoveVerzoek (WagenParkVerzoek verzoek);
}