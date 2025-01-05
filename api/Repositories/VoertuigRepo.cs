using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class VoertuigRepo : IVoertuigService
    {
        private readonly ApplicationDbContext _context;

        public VoertuigRepo(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Voertuig>> GetAllVoertuigen()
        {
            return await _context.Voertuig.ToListAsync();
        }

        public async Task<List<Voertuig>> GetVoertuigenByMerk(string VoertuigMerk)
        {
            return await _context.Voertuig.Where(x => x.Merk == VoertuigMerk).ToListAsync();
        }

        public async Task<List<Voertuig>> GetVoertuigenBySoort(string VoertuigSoort)
        {
            return await _context.Voertuig.Where(x => x.Soort == VoertuigSoort).ToListAsync();
        }
    }
}
