using api.Migrations;
using api.Models;

namespace api.Interfaces;

public interface IWagenparkVerzoekService
{
    Task<bool> AddUserToWagenPark (WagenParkVerzoek verzoek);
    Task<bool> DeclineUserToWagenPark (WagenParkVerzoek verzoek);
    Task<List<AppUser>> GetAllUsers (int id);
    Task<List<WagenParkVerzoek>> GetAllVerzoeken (int wagenparkId);
    Task<bool> RemoveVerzoek (WagenParkVerzoek verzoek);
}