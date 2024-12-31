using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Migrations;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class WagenparkService : IWagenparkService
    {
        private readonly ApplicationDbContext _context;
        public WagenparkService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WagenPark> CreateWagenparkAsync(WagenPark wagenPark, string username)
        {
            var user = await GetUserId(username);
            wagenPark.AppUser = user;
            await _context.AddAsync(wagenPark);
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
            var gevondenwagenpark = await _context.wagenPark
                .SingleOrDefaultAsync(w => w.BedrijfsString.Equals(emailString));
            return gevondenwagenpark == null ? throw new ArgumentException($"geen wagenpark gevonden met emailstring: {email}") : gevondenwagenpark;
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
        await _context.wagenparkVerzoeken.AddAsync(wagenparkVerzoek);
        await _context.SaveChangesAsync();
        return true;
        }
        catch 
        {
            return false;
        }
}
    }
}