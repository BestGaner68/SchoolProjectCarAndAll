using System.Reflection.Metadata.Ecma335;
using api.Data;
using api.Dtos;
using api.Dtos.ReserveringenEnSchade;
using api.Interfaces;
using api.Mapper;
using api.Migrations;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class WagenParkBeheer : IWagenparkVerzoekService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IVoertuigService _voertuigService;
    private readonly IWagenparkService _wagenParkService; 
        public WagenParkBeheer(ApplicationDbContext context, UserManager<AppUser> usermanger, IVoertuigService voertuigService, IWagenparkService wagenparkService)
        {
            _context = context;
            _userManager = usermanger;
            _voertuigService = voertuigService;
            _wagenParkService = wagenparkService; 
        }

    
    public async Task<bool> AcceptUserRequest(int verzoekId, string AppUserId)
    {
        var verzoek = await _context.WagenparkVerzoeken.FindAsync(verzoekId);
        if (verzoek == null || verzoek.Status != "pending")
        {
            return false; 
        }
        var WagenPark = await _context.Wagenpark.FindAsync(verzoek.WagenparkId);
        if (!(WagenPark.AppUser.Id == AppUserId))
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


    public async Task<bool> DenyUserRequest(int verzoekId, string AppUserId)
    {
        var verzoek = await _context.WagenparkVerzoeken.FindAsync(verzoekId);
        if (verzoek == null || verzoek.Status != "Pending")
        {
            return false; 
        }
        var WagenPark = await _context.Wagenpark.FindAsync(verzoek.WagenparkId);
        if (!(WagenPark.AppUser.Id == AppUserId))
        {
            return false;
        }
        verzoek.Status = "Denied";
        _context.WagenparkVerzoeken.Update(verzoek);
        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<List<AppUser>> GetAllUsers(string WagenparkBeheerderId)
    {
        var CurrentWagenPark = await _wagenParkService.GetUsersWagenPark(WagenparkBeheerderId);
        if (CurrentWagenPark == null)
        {
            return [];
        }

        var appUsers = await _context.WagenparkUserLinked
            .Where(w => w.WagenparkId == CurrentWagenPark.WagenParkId)
            .Select(w => w.AppUser)
            .ToListAsync();

        return appUsers;
    }   

    public async Task<List<WagenParkVerzoek>> GetAllVerzoeken(string UserId)
    {
        var CurrentWagenPark = await _wagenParkService.GetUsersWagenPark(UserId);
        if (CurrentWagenPark == null)
        {
            return [];
        }
        int WagenParkId = CurrentWagenPark.WagenParkId;
        var verzoeken = await _context.WagenparkVerzoeken.Where(w => w.WagenparkId == WagenParkId).ToListAsync();
        return verzoeken;
    }

    public async Task<List<WagenParkOverzichtDto>> GetOverzicht(string wagenparkbeheerderId)
    {
        var usersInWagenPark = await GetAllUsers(wagenparkbeheerderId);
        var userIds = usersInWagenPark.Select(user => user.Id).ToList();
        var reserveringenBinnenWagenPark = await _context.Reservering
            .Where(r => userIds.Contains(r.AppUserId))
            .ToListAsync();
        var wagenParkOverzichtDtos = new List<WagenParkOverzichtDto>();
        foreach (var reservering in reserveringenBinnenWagenPark)
        {
            var voertuigData = await _voertuigService.GetAllVoertuigDataById(reservering.VoertuigId);
            var appUser = await _context.Users.FindAsync(reservering.AppUserId);
            var voertuigStatus = await _voertuigService.GetStatus(reservering.VoertuigId);
            if (voertuigData != null && appUser != null)
            {
                var dto = WagenParkMapper.ToOverzichtDto(reservering, voertuigData, appUser, voertuigStatus);
                wagenParkOverzichtDtos.Add(dto);
            }
        }
        return wagenParkOverzichtDtos;
    }

    public async Task<bool> RemoveUser(string AppUserId, string WagenParkOwnerId)
    {
        var wagenParkOwner = await _context.Users.FindAsync(WagenParkOwnerId);
        var wagenPark = await _context.Wagenpark
            .Where(w => w.AppUser == wagenParkOwner) 
            .FirstOrDefaultAsync();
        if (wagenPark == null)
        {
            return false; 
        }
        var userLink = await _context.WagenparkUserLinked
            .Where(l => l.WagenparkId == wagenPark.WagenParkId && l.AppUserId == AppUserId)
            .FirstOrDefaultAsync();
        if (userLink == null)
        {
            return false; 
        }

        _context.WagenparkUserLinked.Remove(userLink);
        await _context.SaveChangesAsync();
        return true; 
    }

    public async Task<bool> RemoveVerzoek(WagenParkVerzoek verzoek)
    {
        _context.WagenparkVerzoeken.Remove(verzoek);
        await _context.SaveChangesAsync();
        return true;
    }
}
