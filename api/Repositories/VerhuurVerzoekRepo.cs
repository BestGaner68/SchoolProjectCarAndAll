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
            return await _context.VerhuurVerzoek
                .Include(v => v.Verzekering) // Ensure Verzekering is included
                .Include(v => v.Accessoires) // Ensure Accessoires are included (if needed)
                .FirstOrDefaultAsync(v => v.VerhuurVerzoekId == id);
        }

         public async Task<VerhuurVerzoek> CreateAsync(VerhuurVerzoek verhuurVerzoekModel)
        {
            await _context.VerhuurVerzoek.AddAsync(verhuurVerzoekModel);
            await _context.SaveChangesAsync();
            return verhuurVerzoekModel;
        }

        public async Task<List<VolledigeDataDto>> GetPendingAsync()
        {
            var verhuurVerzoeken = await _context.VerhuurVerzoek
                .Where(v => v.Status == "Pending")
                .Include(v => v.Accessoires)    
                .Include(v => v.Verzekering)
                .ToListAsync();

            var result = new List<VolledigeDataDto>();

            foreach (var verhuurVerzoek in verhuurVerzoeken)
            {
                AppUser user = await _context.Users.FindAsync(verhuurVerzoek.AppUserId)
                    ?? throw new Exception("Geen gebruiker gevonden");
                string fullName = $"{user.Voornaam} {user.Achternaam}";
                VoertuigDto voertuigDto = await _voertuigService.GetSimpleVoertuigDataById(verhuurVerzoek.VoertuigId);
                var volledigeDataDto = VerhuurVerzoekMapper.ToVolledigeDataDto(verhuurVerzoek, fullName, voertuigDto);
                volledigeDataDto.Accessoires = verhuurVerzoek.Accessoires.Select(a => a.Naam).ToList(); 
                volledigeDataDto.Verzekering = verhuurVerzoek.Verzekering?.VerzekeringNaam; 

                result.Add(volledigeDataDto);
            }

            return result;
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

        public async Task<List<Verzekering>> GetAllVerzekeringen()
        {
            return await _context.Verzekeringen.ToListAsync();
        }

        public async Task<List<Accessoires>> GetAllAccessoires()
        {
            return await _context.Accessoires.ToListAsync();
        }

        public async Task<List<Accessoires>> FromIdToInstanceAccessoires(List<int?> AccessoiresList)
        {
            if (AccessoiresList.Count == 0)
            {
                return [];
            }
            List<Accessoires> GekozenAccesoires = [];
            foreach (int? accessoire in AccessoiresList)
            {
                var ToAdd = await _context.Accessoires.FindAsync(accessoire);
                if (ToAdd != null)
                {
                    GekozenAccesoires.Add(ToAdd);
                }
            }
            return GekozenAccesoires;
        }

        public async Task<Verzekering> FromIdToInstanceVerzekering(int verzekeringId)
        {
            return await _context.Verzekeringen.FindAsync(verzekeringId) ?? throw new ArgumentException($"Er is geen Verzekering gevonden met verzekeringsId: {verzekeringId}");
        }

        public async Task<VerhuurVerzoek> GetByIdOverzichtVerhuurverzoek(int VerhuurverzoekId)
        {
            return await _context.VerhuurVerzoek.Include(x => x.Verzekering)
                .Include(x => x.Accessoires)
                .FirstOrDefaultAsync(x => x.VerhuurVerzoekId == VerhuurverzoekId)
                ?? throw new ArgumentException("Geen OverzichtData Gevonden");
        }

    }
}