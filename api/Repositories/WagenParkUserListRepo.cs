using System.Reflection.Metadata.Ecma335;
using api.Data;
using api.DataStructureClasses;
using api.Dtos;
using api.Dtos.ReserveringenEnSchade;
using api.Interfaces;
using api.Mapper;

using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class WagenParkUserListRepo : IWagenParkUserListService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IVoertuigService _voertuigService;
    private readonly IEmailService _emailService;

        public WagenParkUserListRepo(ApplicationDbContext context, UserManager<AppUser> usermanger, IVoertuigService voertuigService, IEmailService emailService)
        {
            _context = context;
            _userManager = usermanger;
            _voertuigService = voertuigService;
            _emailService = emailService;
        }

    public async Task<WagenPark> GetWagenParkByAppUserEmail(string email)
    {
        var wagenparkuserlist = await _context.WagenParkUserLists
            .Where(x => x.EmailAddress.ToLower().Equals(email.ToLower()))
            .FirstOrDefaultAsync()
            ?? throw new ArgumentException ("Geen WagenParkUserList gevonden bij deze gebruiker123??");

        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("Email address cannot be null or empty.");
        }
        
        var gevondenWagenPark = await _context.Wagenpark.FindAsync(wagenparkuserlist.WagenParkId) 
            ?? throw new ArgumentException ("Geen WagenPark gevonden bij deze gebruiker??");

        return gevondenWagenPark;
    }

    public async Task<List<AppUser>> GetAllUsersInWagenPark(string WagenParkBeheerderId)
    {
        var currentWagenpark = await _context.Wagenpark
            .FirstOrDefaultAsync(x => x.AppUser.Id.Equals(WagenParkBeheerderId));

        if (currentWagenpark == null)
        {
            return []; 
        }

        var appUserIds = await _context.WagenParkUserLists
            .Where(x => x.WagenPark == currentWagenpark && x.Status == WagenParkUserListStatussen.Toegevoegt)
            .Select(x => x.AppUserId)
            .ToListAsync();

        var tasks = appUserIds.Select(appUserId => _userManager.FindByIdAsync(appUserId));
        var gevondenUsers = await Task.WhenAll(tasks);

        return gevondenUsers.Where(user => user != null).ToList();
    }

    public async Task<List<WagenParkOverzichtDto>> GetOverzicht(string wagenparkbeheerderId)
    {
        var usersInWagenPark = await GetAllUsersInWagenPark(wagenparkbeheerderId);
        var userIds = usersInWagenPark.Select(user => user.Id).ToList();
        var reserveringenBinnenWagenPark = await _context.Reservering
            .Where(r => userIds.Contains(r.AppUserId))
            .ToListAsync();
        var wagenParkOverzichtDtos = new List<WagenParkOverzichtDto>();
        foreach (var reservering in reserveringenBinnenWagenPark)
        {
            var voertuigData = await _voertuigService.GetSimpleVoertuigDataById(reservering.VoertuigId);
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

    public async Task<bool> StuurInvite(string email, string WagenParkBeheerderId)
    {
        var FoundWagenPark = await _context.Wagenpark.Where(x => x.AppUser.Id == WagenParkBeheerderId).FirstOrDefaultAsync();
        if (FoundWagenPark == null)
        {
            return false;
        }
        var dubbelUserInvite = await _context.WagenParkUserLists.Where(x => x.EmailAddress == email && x.WagenParkId == FoundWagenPark.WagenParkId).FirstOrDefaultAsync();
        if (!(dubbelUserInvite == null))
        {
            throw new Exception("Gebruiker is al toegevoegt");
        }


        WagenParkUserList TempWagenParkUserList = new()
        {
            EmailAddress = email,
            WagenPark = FoundWagenPark,
            Status = WagenParkUserListStatussen.Uitgenodigt
        };
        await _context.WagenParkUserLists.AddAsync(TempWagenParkUserList);
        await _context.SaveChangesAsync();
    
        var emailMetaData = new EmailMetaData
        {
            ToAddress = email,
            Subject = "Uitnodiging voor deelname aan WagenPark",
            Body = EmailTemplates.GetUitnodigingVoorWagenparkBody(FoundWagenPark.Bedrijfsnaam)
        };
        await _emailService.SendEmail(emailMetaData);
    
        return true;
    }
    public async Task VerwijderGebruiker(string AppUserId)
    {
        await UpdateUserStatus(AppUserId, WagenParkUserListStatussen.Verwijderd);
        //doe hier nog iets mee bij het maken van een verzoek
    }
    
    public async Task<bool>PrimeUserInWagenParkUserList(string AppUserId, string NieuwStatus, string userEmail, int wagenparkId)
    {
        var result =await _context.WagenParkUserLists.Where(x => x.WagenParkId == wagenparkId && x.EmailAddress == userEmail).FirstOrDefaultAsync();
        if (result == null){
            return false;
        }
        result.AppUserId = AppUserId;
        result.Status = NieuwStatus;
        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<bool>UpdateUserStatus(string AppUserId, string NieuwStatus)
    {
        var result =await _context.WagenParkUserLists.Where(x => x.AppUserId == AppUserId).FirstOrDefaultAsync();
        if (result == null){
            return false;
        }
        result.Status = NieuwStatus;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<WagenPark> GetWagenParkByAppUserId(string appUserId)
    {
        var wagenparkId = await _context.WagenParkUserLists
            .Where(x => x.AppUserId == appUserId)
            .Select(x => x.WagenParkId)
            .FirstOrDefaultAsync();
        if (wagenparkId == 0)
        {
            throw new ArgumentException("Geen wagenpark gevonden voor deze gebruiker.");
        }
        var wagenpark = await _context.Wagenpark.FindAsync(wagenparkId) ?? throw new InvalidOperationException("Het gevonden WagenParkId bestaat niet in de database.");
        return wagenpark;
    }
}
