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

       public async Task<bool> KiesAbonnement(int AbonnementId, string WagenParkEigenaar, DateTime StartDatum, DateTime EindDatum)
        {
            WagenPark? currentWagenPark = await _wagenparkService.GetBeheerdersWagenPark(WagenParkEigenaar);

            if (currentWagenPark == null)
            {
                return false;
            }

            var currentLine = await _context.AbonnementWagenparkLinked
                .FirstOrDefaultAsync(x => x.WagenParkId == currentWagenPark.WagenParkId);

            if (currentLine == null)
            {
                return false;
            }

            currentLine.AbonnementId = AbonnementId;
            currentLine.StartDatum = StartDatum;
            currentLine.EindDatum = EindDatum;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Abonnement>> getAllAbonnementen()
        {
            return await _context.Abonnementen.ToListAsync();
        }

        
    }
}