using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.WagenParkDtos;
using api.Interfaces;
using api.Mapper;
using api.Models;
using api.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class WagenparkRepo : IWagenparkService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;  
        public WagenparkRepo(ApplicationDbContext context, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<WagenPark> CreateWagenparkAsync(WagenPark wagenPark, string userId)
        {
            var user = await _context.Users.FindAsync(userId) ?? throw new ArgumentException("Geen gebruiker gevonden");
            wagenPark.AppUser = user;
            await _context.AddAsync(wagenPark);
            await _context.SaveChangesAsync();
            var abonnementWagenparkLinked = new AbonnementWagenparkLinked
            {
                WagenParkId = wagenPark.WagenParkId,
                AbonnementId = 1,
            };
            await _context.AddAsync(abonnementWagenparkLinked);
            await _context.SaveChangesAsync();
            return wagenPark;
        }

        public async Task<AppUser> GetUserId(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            return user == null ? throw new ArgumentException($"User with username {username} does not exist.") : user;
        }
     
        public async Task<bool> CreateWagenParkVerzoek(string userId, int wagenparkId)
        {
        try
            {
            var wagenparkVerzoek = new WagenParkVerzoek
            {
                WagenparkId = wagenparkId,
                AppUserId = userId,
                Status = "pending"
                
            };  
        await _context.WagenparkVerzoeken.AddAsync(wagenparkVerzoek);
        await _context.SaveChangesAsync();
        return true;
        }
        catch 
        {
            return false;
        }
}

        public async Task<WagenPark?> GetBeheerdersWagenPark(string appUserId)
        {
            AppUser? currentAppUser = await _context.Users.FindAsync(appUserId);
            if (currentAppUser == null)
            {
                return null;
            }
            var foundWagenPark = await _context.Wagenpark
                .Where(w => w.AppUser == currentAppUser)
                .FirstOrDefaultAsync();

            if (foundWagenPark == null)
            {
                return null;
            }

            return foundWagenPark;
        }

        public async Task<WagenPark?> GetAppUsersWagenpark(string AppUserId)
        {
            var UserWagenpark = await _context.WagenparkUserLinked
                .Where(x => x.AppUserId == AppUserId)
                .Select(w => w.WagenparkId)
                .FirstOrDefaultAsync();

            var ThisWagenpark = await _context.Wagenpark.FindAsync(UserWagenpark); 

            return ThisWagenpark;  
        }

        public async Task<List<AppUser>> GetAllUsers(string WagenparkBeheerderId)
        {
            var CurrentWagenPark = await GetBeheerdersWagenPark(WagenparkBeheerderId);
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

        public async Task NieuwWagenParkVerzoek(NieuwWagenParkVerzoekDto wagenParkVerzoekDto)
        {
            var nieuwWagenparkVerzoek = WagenParkMapper.ToNieuwWagenParkVerzoekVanNieuwWagenParkDto(wagenParkVerzoekDto);
            await _context.NieuwWagenParkVerzoek.AddAsync(nieuwWagenparkVerzoek);
            await _context.SaveChangesAsync();
        }

        public async Task<List<NieuwWagenParkVerzoek>> GetAllAsync()
        {
            return await _context.NieuwWagenParkVerzoek.ToListAsync();
        }

        public async Task<WagenPark> AcceptNieuwWagenParkVerzoek(int id)
        {
            var verzoek = await _context.NieuwWagenParkVerzoek.FindAsync(id) ?? throw new ArgumentException("Verzoek niet gevonden");

            var password = RandomPasswordService.GenerateRandomPassword();
    
            var newUser = new AppUser
            {
                UserName = verzoek.GewensdeUsername,
                Email = verzoek.Email,
                Voornaam = verzoek.Voornaam,
                Achternaam = verzoek.Achternaam
            };
    
            var result = await _userManager.CreateAsync(newUser, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Er is iets misgegaan bij het aanmaken van de gebruiker: {errors}");
            }

            await _emailService.SendWagenParkBeheerderWelcomeEmail(verzoek.Email, verzoek.GewensdeUsername, password);
            var wagenpark = new WagenPark
            {
                Bedrijfsnaam = verzoek.Bedrijfsnaam,
                KvkNummer = verzoek.KvkNummer
            };
    
            wagenpark = await CreateWagenparkAsync(wagenpark, newUser.Id);
    
            return wagenpark;
        }

        public async Task<bool> WeigerNieuwWagenParkVerzoek(WeigerNieuwWagenParkVerzoekDto weigerNieuwWagenParkVerzoekDto)
        {
            var nieuwWagenParkVerzoek = await _context.NieuwWagenParkVerzoek.FindAsync(weigerNieuwWagenParkVerzoekDto.WagenParkId);
            if (nieuwWagenParkVerzoek == null)
            {
                return false;
            }
            _context.NieuwWagenParkVerzoek.Remove(nieuwWagenParkVerzoek);
            await _context.SaveChangesAsync();
            await _emailService.SendWagenParkBeheerWeigerEmail(nieuwWagenParkVerzoek.Email, weigerNieuwWagenParkVerzoekDto.Reden, nieuwWagenParkVerzoek.Voornaam);
            return true;
        }

        public async Task<WagenPark> GetWagenParkById(int WagenparkId)
        {
            return await _context.Wagenpark.FindAsync(WagenparkId) ?? throw new ArgumentException($"Geen WagenPark Gevonden met Id {WagenparkId}");
        }
    }
}