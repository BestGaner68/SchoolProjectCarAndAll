using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class WagenparkService : IWagenparkService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;  
        public WagenparkService(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<WagenPark> CreateWagenparkAsync(WagenPark wagenPark, string userId)
        {
            var user = await _context.Users.FindAsync(userId);
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

        public async Task<WagenPark> GetWagenParkByEmail(string email)
        {   
            var emailString = email.Split('@').LastOrDefault();
            if (string.IsNullOrEmpty(emailString))
            {
                throw new ArgumentException($"Invalid email format: {email}");
            }
            var gevondenwagenpark = await _context.Wagenpark
                .SingleOrDefaultAsync(w => w.BedrijfsString.Equals(emailString));
            return gevondenwagenpark ?? throw new ArgumentException($"geen wagenpark gevonden met emailstring: {email}");
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
    }
}