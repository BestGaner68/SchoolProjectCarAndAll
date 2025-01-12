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
    public class AbonnementServiceRepo :IAbonnementService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWagenparkService _wagenparkService;
        public AbonnementServiceRepo(ApplicationDbContext context, IWagenparkService wagenparkService)
        {
            _context=context;
            _wagenparkService = wagenparkService;
        }

        public async Task<bool> ChangeAbonnement(int AbonnementId)
        {
            return true;
        }

        public async Task<List<Abonnement>> getAllAbonnementen()
        {
            return await _context.Abonnementen.ToListAsync();
        }
    }
}