using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Verhuur;
using api.Dtos.Voertuig;
using api.Interfaces;
using api.Mapper;
using api.Migrations;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;


namespace api.Repositories
{
    public class VoertuigHelperRepo : IVoertuigHelper
    {
        private readonly ApplicationDbContext _context;
        private readonly IVoertuigService _voertuigHelper;
    
        public VoertuigHelperRepo(ApplicationDbContext context, IVoertuigService voertuigHelper)
        {
            _context = context;
            _voertuigHelper = voertuigHelper;
        }

        public async Task<bool> ChangeStatusVoertuig(int voertuigId, string status)
        {
            var voertuig = await _context.VoertuigStatus.FindAsync(voertuigId);
            if (voertuig == null){
                return false;
            }
            voertuig.status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckDatesAsync(int voertuigId, DateTime startDate, DateTime endDate)
        {
            Console.WriteLine($"Check: VehicleId = {voertuigId}, StartDate = {startDate}, EndDate = {endDate}");
        
            var conflictingReservation = await _context.Reservering
                .Where(r => r.VoertuigId == voertuigId &&
                            r.StartDatum < endDate &&
                            r.EindDatum > startDate)
                .FirstOrDefaultAsync();
        
            if (conflictingReservation != null)
            {
                Console.WriteLine($"Conflict found: {conflictingReservation.StartDatum} - {conflictingReservation.EindDatum}");
            }
        
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

        public async Task<List<DateTime>> GetUnavailableDates(int voertuigId)
        {
            var unavailableDates = await _context.Reservering
            .Where(v => v.VoertuigId == voertuigId)
            .Select(v => new { v.StartDatum, v.EindDatum })
            .ToListAsync();

            var allDates = new List<DateTime>();
            foreach (var dateRange in unavailableDates)
            {
                var currentDate = dateRange.StartDatum;
                while (currentDate <= dateRange.EindDatum)
                {
                    allDates.Add(currentDate);
                    currentDate = currentDate.AddDays(1);
                }
            }
            return allDates;

        }

        public async Task<VolledigeDataDto> GetVolledigeDataDto(VerhuurVerzoek verhuurVerzoek)
        {
            AppUser user = await _context.Users.FindAsync(verhuurVerzoek.AppUserId);
            string fullName = $"{user.Voornaam} {user.Achternaam}";
            VoertuigDto voertuigDto = await _voertuigHelper.GetAllVoertuigDataById(verhuurVerzoek.VoertuigId);
            VolledigeDataDto volledigeDataDto = VerhuurVerzoekMapper.ToVolledigeDataDto(verhuurVerzoek, fullName, voertuigDto);
            return volledigeDataDto;
        }
    }
}
