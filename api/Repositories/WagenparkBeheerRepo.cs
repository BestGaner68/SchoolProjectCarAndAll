using api.Data;
using api.Dtos;
using api.Interfaces;
using api.Migrations;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class WagenParkBeheer : IWagenparkVerzoekService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager; 
        public WagenParkBeheer(ApplicationDbContext context, UserManager<AppUser> usermanger)
        {
            _context = context;
            _userManager = usermanger;
        }

    
    public async Task<bool> AcceptUserRequest(int verzoekId)
    {
        var verzoek = await _context.WagenparkVerzoeken.FindAsync(verzoekId);
        if (verzoek == null || verzoek.Status != "pending")
        {
            return false; 
        }
        
        verzoek.Status = "Accepted";
        _context.WagenparkVerzoeken.Update(verzoek);
        var appUser = await _userManager.FindByIdAsync(verzoek.AppUserId.ToString());
        
        var currentRoles = await _userManager.GetRolesAsync(appUser);
        if (currentRoles.Contains("pending"))
        {
            var removeRoleResult = await _userManager.RemoveFromRoleAsync(appUser, "pending");
            if (!removeRoleResult.Succeeded)
            {
                return false;  
            }
        }

        var addRoleResult = await _userManager.AddToRoleAsync(appUser, "bedrijfsKlant");
        if (!addRoleResult.Succeeded)
        {
            return false;  
        }

        var WagenParkID = await _context.WagenparkVerzoeken.Where(x => x.wagenparkverzoekId == verzoekId).Select(x => x.WagenparkId).FirstOrDefaultAsync();

        if (WagenParkID == 0) 
        {
            return false;
        }

        WagenparkLinkedUser wagenparklinkeduser = new()
        {
            AppUserId = appUser.Id,
            WagenparkId = WagenParkID
        };

        var succes = await _context.WagenparkUserLinked.AddAsync(wagenparklinkeduser);
        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<bool> DenyUserRequest(int verzoekId)
    {
        var verzoek = await _context.WagenparkVerzoeken.FindAsync(verzoekId);
        if (verzoek == null || verzoek.Status != "Pending")
        {
            return false; 
        }
        verzoek.Status = "Denied";
        _context.WagenparkVerzoeken.Update(verzoek);
        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<List<AppUser>> GetAllUsers(int Id)
    {
        WagenPark currentWagenPark = await _context.Wagenpark.FindAsync(Id);
        if (currentWagenPark == null)
        {
            return [];
        }

        var appUsers = await _context.WagenparkUserLinked
            .Where(w => w.WagenparkId == currentWagenPark.WagenParkId)
            .Select(w => w.AppUser)
            .ToListAsync();

        return appUsers;
    }   

    public async Task<List<WagenParkVerzoek>> GetAllVerzoeken(string UserId)
    {
        var CurrentUser = await _context.Users.FindAsync(UserId);
        int WagenParkId = await _context.Wagenpark.Where(w => w.AppUser == CurrentUser).Select(w => w.WagenParkId).FirstOrDefaultAsync();
        var verzoeken = await _context.WagenparkVerzoeken.Where(w => w.WagenparkId == WagenParkId).ToListAsync();
        if (verzoeken == null) {
            Console.WriteLine("geen verzoeken al hier");
        }
        return verzoeken;
    }

    public async Task<bool> RemoveVerzoek(WagenParkVerzoek verzoek)
    {
        _context.WagenparkVerzoeken.Remove(verzoek);
        await _context.SaveChangesAsync();
        return true;
    }
}
