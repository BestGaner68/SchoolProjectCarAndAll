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
    public class VerhuurVerzoekRepo : IVerhuurVerzoekService
    {
        private readonly ApplicationDbContext _context;
        public VerhuurVerzoekRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<VerhuurVerzoek>> GetAllAsync()
        {
            return  _context.verhuurVerzoek.ToListAsync();
        }
    }
}