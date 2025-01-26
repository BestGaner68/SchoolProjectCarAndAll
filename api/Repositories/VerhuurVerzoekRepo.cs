using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Verhuur;
using api.Dtos.VoertuigDtos;
using api.Interfaces;
using api.Mapper;
using api.Models;
using Microsoft.EntityFrameworkCore;


namespace api.Repositories
{
    public class VerhuurVerzoekRepo : IVerhuurVerzoekService
    {
        private readonly ApplicationDbContext _context;
        private readonly IVoertuigService _voertuigService;
        public VerhuurVerzoekRepo(ApplicationDbContext context, IVoertuigService voertuigService)
        {
            _context = context;
            _voertuigService = voertuigService;
        }

        public Task<List<VerhuurVerzoek>> GetAllAsync()
        {
            return  _context.VerhuurVerzoek.ToListAsync();
        }

        public async Task<VerhuurVerzoek?> GetByIdAsync(int id)
        {
            return await _context.VerhuurVerzoek.FindAsync(id);
        }

         public async Task<VerhuurVerzoek> CreateAsync(VerhuurVerzoek verhuurVerzoekModel)
        {
            await _context.VerhuurVerzoek.AddAsync(verhuurVerzoekModel);
            await _context.SaveChangesAsync();
            return verhuurVerzoekModel;
        }

        public async Task<List<VerhuurVerzoek>> GetPendingAsync()
        {
            return await _context.VerhuurVerzoek
                .Where(v => v.Status == "Pending")
                .ToListAsync();
        }

        public async Task<VolledigeDataDto> GetVolledigeDataDto(VerhuurVerzoek verhuurVerzoek)
        {
            AppUser user = await _context.Users.FindAsync(verhuurVerzoek.AppUserId);
            string fullName = $"{user.Voornaam} {user.Achternaam}";
            VoertuigDto voertuigDto = await _voertuigService.GetSimpleVoertuigDataById(verhuurVerzoek.VoertuigId);
            VolledigeDataDto volledigeDataDto = VerhuurVerzoekMapper.ToVolledigeDataDto(verhuurVerzoek, fullName, voertuigDto);
            return volledigeDataDto;
        }

        public async Task<bool> DeclineMyVerzoek(int verhuurVerzoekId, string AppUserId)
            {
            var currentVerhuurVerzoek = await _context.VerhuurVerzoek.FindAsync(verhuurVerzoekId);
            if (currentVerhuurVerzoek == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(AppUserId) || !currentVerhuurVerzoek.AppUserId.Equals(AppUserId))
            {
                return false;
            }
            currentVerhuurVerzoek.Status = "Door gebruiker verwijderd"; 
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<VerhuurVerzoek>> GetMyVerhuurVerzoeken(string AppUserId)
        {
            return await _context.VerhuurVerzoek
            .Where(v => v.AppUserId == AppUserId)
            .ToListAsync();
        }

        public async Task<List<Reservering>> ViewHuurGeschiedenis(string AppUserId)
        {
            var Verzoeken = await _context.Reservering
                .Where(v => v.AppUserId == AppUserId && v.Status == "Afgerond")
                .ToListAsync();
            return Verzoeken;
        }
    }
}