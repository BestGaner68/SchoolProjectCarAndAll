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

    public async Task<bool> AddUserToWagenPark(WagenParkVerzoek verzoek)
    {
        WagenparkLinkedUser linkedUser = new WagenparkLinkedUser{
            Wagenpark = verzoek.wagenPark,
            AppUser = verzoek.appUser,
        };
        await _context.wagenparkUserLinked.AddAsync(linkedUser);
        await _context.SaveChangesAsync();
        await RemoveVerzoek(verzoek);
        //logger??
        return true;
    }

    public async Task<bool> DeclineUserToWagenPark(WagenParkVerzoek verzoek)
    {
        await RemoveVerzoek(verzoek);
        //logger?
        //notifyUser?
        return true;
    }

    public async Task<List<AppUser>> GetAllUsers(int Id)
{
    WagenPark currentWagenPark = await _context.wagenPark.FindAsync(Id);
    if (currentWagenPark == null)
    {
        return [];
    }

    var appUsers = await _context.wagenparkUserLinked
        .Where(w => w.WagenparkId == currentWagenPark.WagenParkId)
        .Select(w => w.AppUser)
        .ToListAsync();

    return appUsers;
}

    public async Task<List<WagenParkVerzoek>> GetAllVerzoeken(int Id)
    {
        var verzoeken = await _context.wagenparkVerzoeken.Where(w => w.WagenparkId == Id).ToListAsync();
        if (verzoeken == null) {
            Console.WriteLine("geen verzoeken al hier");
        }
        return verzoeken;
    }

    public async Task<bool> RemoveVerzoek(WagenParkVerzoek verzoek)
    {
        _context.wagenparkVerzoeken.Remove(verzoek);
        await _context.SaveChangesAsync();
        return true;
    }
}
