using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DataStructureClasses;
using api.Dtos.Verhuur;
using api.Dtos.VoertuigDtos;
using api.Interfaces;
using api.Mapper;

using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;


namespace api.Repositories
{
    public class VoertuigServiceRepo : IVoertuigService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWagenparkService _wagenparkService;
    
        public VoertuigServiceRepo(ApplicationDbContext context, IWagenparkService wagenparkService)
        {
            _context = context;
            _wagenparkService = wagenparkService;
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

        public async Task<bool> IsAvailable(int voertuigId)
        {
            var status = await _context.VoertuigData
                .Where(v => v.VoertuigId == voertuigId)
                .Select(v => v.Status)
                .FirstOrDefaultAsync();
            
            if (status == null)
            {
                return false;
            }
    
            if (!(status == VoertuigStatussen.KlaarVoorGebruik))
            {
                return false;
            }
            return true;
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

        public async Task<VoertuigDto> GetSimpleVoertuigDataById(int voertuigId)
        {
            var voertuig = await _context.Voertuig.FindAsync(voertuigId) ?? throw new InvalidDataException();
            return new VoertuigDto{
                Soort = voertuig.Soort,
                type = voertuig.Type,
                Merk = voertuig.Merk,
            };
        }

        public async Task<VolledigeVoertuigDataDto> GetAllDataVoertuig(int voertuigId)
        {
            var voertuig = await _context.Voertuig
                .Include(v => v.VoertuigData)
                .FirstOrDefaultAsync(v => v.VoertuigId == voertuigId) ?? throw new KeyNotFoundException($"Voertuig with ID {voertuigId} not found.");

            var schades = await _context.SchadeFormulier
                .Where(s => s.VoertuigId == voertuigId)
                .ToListAsync();

            return new VolledigeVoertuigDataDto
            {
                Voertuig = voertuig,
                Schades = schades
            };
        }

        public async Task<List<Voertuig>> GetAllVoertuigen()
        {
            return await _context.Voertuig.Include(v => v.VoertuigData).ToListAsync();
        }

        public async Task<List<Voertuig>> GetVoertuigenByMerk(string VoertuigMerk)
        {
            return await _context.Voertuig.Where(x => x.Merk == VoertuigMerk).ToListAsync();
        }

        public async Task<List<Voertuig>> GetVoertuigenBySoort(string VoertuigSoort)
        {
            return await _context.Voertuig.Where(x => x.Soort == VoertuigSoort).ToListAsync();
        }

        public async Task<List<SchadeFormulier>> GetAllIngediendeFormulieren()
        {
            return await _context.SchadeFormulier.Where(x => x.Status.Equals(SchadeStatus.Ingediend)).ToListAsync();
        }

        public async Task<bool> BehandelSchadeMelding(int schadeformulierId, string ReparatieOpmerking)
        {
            var schadeMelding = await _context.SchadeFormulier.FindAsync(schadeformulierId);
            if (schadeMelding == null)
            {
                return false;
            }
            schadeMelding.ReparatieOpmerking = ReparatieOpmerking;
            schadeMelding.Status = SchadeStatus.Behandeld;

            var CurrentVoertuigStatus = await _context.VoertuigData.Where(x => x.VoertuigId == schadeMelding.VoertuigId).FirstOrDefaultAsync();
            if (CurrentVoertuigStatus == null)
            {
                return false;
            }
            CurrentVoertuigStatus.Status = VoertuigStatussen.InReparatie;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BlokkeerVoertuig(int voertuigId, string? Opmerking)
        {
            var CurrentVoertuig = await _context.VoertuigData.FindAsync(voertuigId);
            if (CurrentVoertuig == null)
            {
                return false;
            }
            CurrentVoertuig.Status = VoertuigStatussen.Geblokkeerd;
            CurrentVoertuig.Opmerking = Opmerking;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeBlokkeerVoertuig(int voertuigId)
        {
            var CurrentVoertuig = await _context.VoertuigData.FindAsync(voertuigId);
            if (CurrentVoertuig == null)
            {
                return false;
            }
            if (!(CurrentVoertuig.Status == VoertuigStatussen.Geblokkeerd))
            {
                return false;
            }
            CurrentVoertuig.Status = VoertuigStatussen.KlaarVoorGebruik;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Voertuig>> GetAllAvailableVoertuigen()
        {
            var VoertuigLijst = await _context.VoertuigData.Where(x => x.Status == VoertuigStatussen.KlaarVoorGebruik).ToListAsync();
            var AvailableVoertuigen = new List<Voertuig>();
            foreach (VoertuigData voertuig in VoertuigLijst)
            {
                var tempVoertuig = await _context.Voertuig.FindAsync(voertuig.VoertuigId);
                if (tempVoertuig != null)
                {
                    AvailableVoertuigen.Add(tempVoertuig);
                }
            }
            return AvailableVoertuigen;
        }

        public async Task<string> GetStatus(int voertuigId)
        {
            var VoertuigStatus = await _context.VoertuigData.FindAsync(voertuigId);
            if (VoertuigStatus == null)
            {
                return $"Geen voertuig gevonden met id {voertuigId}";
            }
            return VoertuigStatus.Status;
        }

        public async Task<bool> CreeerNieuwVoertuig(NieuwVoertuigDto nieuwVoertuigDto)
        {
            var TempVoertuig = VoertuigMapper.FromNieuweVoertuigDtoToVoertuig(nieuwVoertuigDto);
            TempVoertuig.VoertuigData = new VoertuigData{
                Status = VoertuigStatussen.KlaarVoorGebruik,
            };
            await _context.Voertuig.AddAsync(TempVoertuig);
            await _context.SaveChangesAsync();
            return true;
        }

        
        public async Task<bool> WeizigVoertuig(WeizigVoertuigDto weizigVoertuigDto)
            {
            var currentVoertuig = await _context.Voertuig.FindAsync(weizigVoertuigDto.VoertuigId);
            if (currentVoertuig == null)
            {
                return false; 
            }
            
            VoertuigMapper.MapWeizigVoertuigDtoToVoertuig(weizigVoertuigDto, currentVoertuig);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> VerwijderVoertuig(int voertuigId)
        {
            var currentVoertuig = await _context.Voertuig.FindAsync(voertuigId);
            if (currentVoertuig == null)
            {
                return false;
            }
            _context.Voertuig.Remove(currentVoertuig);
            await _context.SaveChangesAsync();
            return true; 
        }

        public async Task AddVoertuigStatuses(List<VoertuigData> voertuigData)
        {
            if (voertuigData.Count == 0)
                throw new ArgumentException("geen voertuigen gevonden");

            await _context.VoertuigData.AddRangeAsync(voertuigData);
            await _context.SaveChangesAsync(); 
        }

        public async Task<bool> AreAnyVoertuigData()
        {
            var anyVoertuigStatus = await _context.VoertuigData.ToListAsync();
            if (anyVoertuigStatus.Count != 0)
            {
                return true;
            }
            return false;
        }

        public async Task AddVoertuigen(List<Voertuig> voertuigen)
        {
            if (voertuigen.Count == 0)
                throw new ArgumentException("geen voertuigen gevonden");

            await _context.Voertuig.AddRangeAsync(voertuigen); 
            await _context.SaveChangesAsync();
        }

        public async Task<List<Voertuig>> GetVoertuigenByDate(Dtos.ReserveringenEnSchade.DatumDto datumDto)
        {
            var startDate = datumDto.StartDate;
            var endDate = datumDto.EndDate;

            var conflicterendeVoertuigIds = await _context.Reservering
                .Where(r => r.StartDatum < endDate && r.EindDatum > startDate) 
                .Select(r => r.VoertuigId) 
                .Distinct() 
                .ToListAsync();

            var conflicterendeVoertuigIds2 = await _context.VerhuurVerzoek
                .Where(r => r.StartDatum < endDate && r.EindDatum > startDate) 
                .Select(r => r.VoertuigId) 
                .Distinct() 
                .ToListAsync();

            
            return await _context.Voertuig
                .Where(v => !conflicterendeVoertuigIds.Contains(v.VoertuigId) && !conflicterendeVoertuigIds2.Contains(v.VoertuigId)) 
                .ToListAsync();
        }

    }
}

