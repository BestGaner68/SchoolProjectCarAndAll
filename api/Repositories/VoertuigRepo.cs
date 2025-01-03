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

        public async Task<List<Auto>> GetAllAutos()
        {
            return await _context.Auto.ToListAsync();
        }

        public async Task<List<Camper>> GetAllCampers()
        {
            return await _context.Camper.ToListAsync();
        }

        public async Task<List<Caravan>> GetAllCaravans()
        {
            return await _context.Caravan.ToListAsync();
        }
    }
}
