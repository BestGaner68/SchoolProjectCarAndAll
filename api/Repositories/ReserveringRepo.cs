using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Mapper;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class ReserveringRepo : IReserveringService
    {
        private readonly ApplicationDbContext _context;
        public ReserveringRepo (ApplicationDbContext context){
            _context = context;
        }
        
        public async Task<bool> AcceptVerhuurVerzoek(int verhuurVerzoekId)
        {
            try
            {
                var CurrentVerhuurVerzoek = await _context.VerhuurVerzoek.FindAsync(verhuurVerzoekId);
                if (CurrentVerhuurVerzoek == null){
                    return false;
                }
                CurrentVerhuurVerzoek.Status = "Geaccpeteerd";
                var CurrentReservering = VerhuurVerzoekMapper.ToReserveringFromVerhuurVerzoek(CurrentVerhuurVerzoek);
                var User = await _context.Users.FindAsync(CurrentReservering.AppUserId);
                CurrentReservering.Fullname = $"{User.Voornaam} {User.Achternaam}";
                await _context.Reservering.AddAsync(CurrentReservering);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false; 
            }
        }

        public async Task<bool> WeigerVerhuurVerzoek(int verhuurVerzoekId)
        {
            try{
                var CurrentVerhuurVerzoek = await _context.VerhuurVerzoek.FindAsync(verhuurVerzoekId);
                if (CurrentVerhuurVerzoek == null) 
                {
                    return false;
                }
                CurrentVerhuurVerzoek.Status = "Afgekeurt";
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Reservering>> GetAll()
        {
           var result = await _context.Reservering.ToListAsync();
           return result;
        }

        public async Task<Reservering> GetById(int ReserveringId)
        {
            try
            {
                var reservering = await _context.Reservering.FindAsync(ReserveringId);
                if (reservering == null)
                {
                    return null;
                }
                return reservering;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}