using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace api.Repositories
{
    public class VoertuigHelperRepo : IVoertuigHelper
    {
        private readonly ApplicationDbContext _context;
    
        public VoertuigHelperRepo(ApplicationDbContext context)
        {
            _context = context;
        }
    
        public async Task<bool> CheckDatesAsync(int voertuigId, DateTime startDate, DateTime endDate)
        {
            var conflictingReservation = await _context.Reservering
                .Where(r => r.VoertuigId == voertuigId &&
                            r.StartDatum <= endDate &&
                            r.EindDatum >= startDate)
                .FirstOrDefaultAsync();
    
            return conflictingReservation == null; 
        }

        public async Task<bool> CheckStatusAsync(int voertuigId)
        {
        var status = await _context.VoertuigStatus
            .Where(v => v.VoertuigId == voertuigId)
            .Select(v => v.status)
            .FirstOrDefaultAsync();

        return status == "Beschikbaar";
        }
    }
}
