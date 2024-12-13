using api.Migrations;
using api.Models;

namespace api.Interfaces;

public interface IWagenparkVerzoekService
{
    Task<bool> AddUserToWagenPark (String appuserId, int WagenParkId);
    Task<bool> DeclineUserToWagenPark (String appuserId, int WagenParkId);
    Task<List<AppUser>> GetAllUsers (int id);
    Task<List<WagenParkVerzoek>> GetAllVerzoeken (int wagenparkId);
}