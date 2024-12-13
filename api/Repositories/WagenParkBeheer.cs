using api.Data;
using api.Interfaces;
using api.Migrations;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class WagenParkBeheer : IWagenparkVerzoekService
{
    private readonly ApplicationDbContext _context;
        public WagenParkBeheer(ApplicationDbContext context)
        {
            _context = context;
        }


    public async Task<bool> AddUserToWagenPark(string appuserId, int WagenParkId)
    {
        WagenPark wagenpark = await _context.wagenPark.FindAsync(WagenParkId)
            ?? throw new ArgumentException("WagenPark not found");
        AppUser appuser = await _context.Users.FindAsync(appuserId) 
            ?? throw new ArgumentException("AppUser not found");
        WagenparkLinkedUser WagenparkConnectie = new WagenparkLinkedUser{
            AppUserId = appuser.Id,
            WagenparkId = wagenpark.WagenParkId,
        };
        await _context.AddAsync(WagenparkConnectie);
        return true;
    }

    public Task<bool> DeclineUserToWagenPark(string appuserId, int WagenParkId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<AppUser>> GetAllUsers(int findwagenparkId)
    {
        WagenPark CurrentWagenPark = await _context.wagenPark.FindAsync(findwagenparkId);
        var appusers = await _context.wagenparkUserLinked
        .Where(w => w.WagenparkId == CurrentWagenPark.WagenParkId)
        .Select(w => w.AppUser)
        .ToListAsync();
        return appusers; 
    }

    public async Task<List<WagenParkVerzoek>> GetAllVerzoeken(int Id)
    {
        return await _context.wagenparkVerzoeken.Where(w => w.WagenparkId == Id)
        .ToListAsync();
    }
}
