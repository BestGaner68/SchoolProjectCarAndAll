using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DataStructureClasses;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class KostenRepo : IKostenService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAbonnementService _abonnementService;
        private readonly int PrijsPerDag = 15;
        private readonly int SchadePrijs = 80;
        public KostenRepo(ApplicationDbContext context, IAbonnementService abonnementService)
        {
            _context = context;
            _abonnementService = abonnementService;
        }

        public async Task<decimal> BerekenVoorschot(int reserveringId, string appuserId)
        {
            var abonnement = await _abonnementService.GetUserAbonnement(appuserId);
            if (abonnement == null)
            {
                return await BerekenVoorschotPrijsParticulier(reserveringId);
            }
            return await BerekenVoorschotPrijsZakelijk(reserveringId, abonnement);
        }

        public async Task<decimal> BerekenDaadWerkelijkPrijs(int reserveringId, int KilometersGereden, bool isSchade, string appuserId)
        {
            var abonnement = await _abonnementService.GetUserAbonnement(appuserId);
            if (abonnement == null)
            {
                return await BerekenDaadwerkelijkePrijsParticulier(reserveringId, KilometersGereden, isSchade);
            }
            return await BerekenDaadwerkelijkePrijsZakelijk(reserveringId, KilometersGereden, isSchade, abonnement);
        }

        public async Task<decimal> BerekenVoorschotPrijsParticulier(int reserveringId)
        {
            var reservering = await _context.Reservering
                .FirstOrDefaultAsync(r => r.ReserveringId == reserveringId) 
                ?? throw new InvalidOperationException("Reservering not found");

            var voertuigData = await _context.VoertuigData
                .FirstOrDefaultAsync(vs => vs.VoertuigId == reservering.VoertuigId) 
                ?? throw new InvalidOperationException("VoertuigData not found");

            var rentalDuration = (reservering.EindDatum - reservering.StartDatum).Days;
            var additionalKilometerCharge = reservering.VerwachtteKM * voertuigData.KilometerPrijs;
            var estimatedPrice = (PrijsPerDag * rentalDuration) + SchadePrijs + additionalKilometerCharge;
            return estimatedPrice;
        }

        public async Task<decimal> BerekenDaadwerkelijkePrijsParticulier(int reserveringId, decimal kilometersDriven, bool isSchade)
        {
            var reservering = await _context.Reservering
                .FirstOrDefaultAsync(r => r.ReserveringId == reserveringId) 
                ?? throw new InvalidOperationException("Reservering not found");

            var voertuigStatus = await _context.VoertuigData
                .FirstOrDefaultAsync(vs => vs.VoertuigId == reservering.VoertuigId) 
                ?? throw new InvalidOperationException("VoertuigData not found");

            var rentalDuration = (reservering.EindDatum - reservering.StartDatum).Days;
            var additionalKilometerCharge = kilometersDriven * voertuigStatus.KilometerPrijs;
            var actualPrice = (PrijsPerDag * rentalDuration) + additionalKilometerCharge;
            if (isSchade)
            {
                actualPrice += SchadePrijs;
            }
            return actualPrice;
        }

        public async Task<decimal> BerekenVoorschotPrijsZakelijk(int reserveringId, Abonnement abonnement)
        {
            var reservering = await _context.Reservering
                .FirstOrDefaultAsync(r => r.ReserveringId == reserveringId)
                ?? throw new InvalidOperationException("Reservering not found");

            var voertuigData = await _context.VoertuigData
                .FirstOrDefaultAsync(v => v.VoertuigId == reservering.VoertuigId)
                ?? throw new InvalidOperationException("VoertuigData not found");

            var rentalDuration = (reservering.EindDatum - reservering.StartDatum).Days;

            return GetPrijsOpBasisVanAbonnement(
                abonnement,
                reservering.VerwachtteKM,
                rentalDuration,
                voertuigData.KilometerPrijs,
                isSchade: false
            );
        }

        public async Task<decimal> BerekenDaadwerkelijkePrijsZakelijk(int reserveringId, decimal kilometersDriven, bool isSchade, Abonnement abonnement)
        {
            var reservering = await _context.Reservering
                .FirstOrDefaultAsync(r => r.ReserveringId == reserveringId)
                ?? throw new InvalidOperationException("Reservering not found");

            var voertuigData = await _context.VoertuigData
                .FirstOrDefaultAsync(v => v.VoertuigId == reservering.VoertuigId)
                ?? throw new InvalidOperationException("VoertuigData not found");

            var rentalDuration = (reservering.EindDatum - reservering.StartDatum).Days;

            return GetPrijsOpBasisVanAbonnement(
                abonnement,
                kilometersDriven,
                rentalDuration,
                voertuigData.KilometerPrijs,
                isSchade
            );
        }

        private decimal GetPrijsOpBasisVanAbonnement(Abonnement abonnement, decimal kilometersDriven, int rentalDuration, decimal kilometerPrijs, bool isSchade)
        {
            return abonnement.Naam switch
            {
                AbonnementNamen.PayAsYouGo => ApplyPayAsYouGoPricing(kilometersDriven, rentalDuration, kilometerPrijs, isSchade),
                AbonnementNamen.Basic => ApplyBasicPricing(kilometersDriven, rentalDuration, kilometerPrijs, isSchade),
                AbonnementNamen.Professional => ApplyProfessionalPricing(kilometersDriven, rentalDuration, kilometerPrijs, isSchade),
                AbonnementNamen.Premium => ApplyPremiumPricing(kilometersDriven, rentalDuration, kilometerPrijs, isSchade),
                _ => throw new InvalidOperationException("Unknown abonnement type")
            };
        }

        private static decimal ApplyPayAsYouGoPricing(decimal kilometersDriven, int rentalDuration, decimal kilometerPrijs, bool isSchade)
        {
            decimal maxDailyKm = 50;
            decimal surchargePerKm = 0.25m;
            decimal dailyRate = 30m;
            decimal basePrice = dailyRate * rentalDuration + kilometersDriven * kilometerPrijs;
            decimal averageDailyKm = kilometersDriven / rentalDuration;
            if (averageDailyKm > maxDailyKm)
            {
                basePrice += (averageDailyKm - maxDailyKm) * surchargePerKm * rentalDuration;
            }
            if (isSchade)
            {
                basePrice += 100m;
            }
            return basePrice;
        }

        private static decimal ApplyBasicPricing(decimal kilometersDriven, int rentalDuration, decimal kilometerPrijs, bool isSchade)
        {
            decimal maxDailyKm = 75;
            decimal surchargePerKm = 0.20m;
            decimal dailyRate = 25m;
            decimal basePrice = dailyRate * rentalDuration + kilometersDriven * kilometerPrijs;
            decimal averageDailyKm = kilometersDriven / rentalDuration;
            if (averageDailyKm > maxDailyKm)
            {
                basePrice += (averageDailyKm - maxDailyKm) * surchargePerKm * rentalDuration;
            }
            if (isSchade)
            {
                basePrice += 80m;
            }
            return basePrice;
        }

        private static decimal ApplyProfessionalPricing(decimal kilometersDriven, int rentalDuration, decimal kilometerPrijs, bool isSchade)
        {
            decimal maxDailyKm = 100;
            decimal surchargePerKm = 0.15m;
            decimal dailyRate = 20m;
            decimal basePrice = dailyRate * rentalDuration + kilometersDriven * kilometerPrijs;
            decimal averageDailyKm = kilometersDriven / rentalDuration;
            if (averageDailyKm > maxDailyKm)
            {
                basePrice += (averageDailyKm - maxDailyKm) * surchargePerKm * rentalDuration;
            }
            basePrice *= 0.9m;
            if (isSchade)
            {
                basePrice += 50m; 
            }
            return basePrice;
        }

        private decimal ApplyPremiumPricing(decimal kilometersDriven, int rentalDuration, decimal kilometerPrijs, bool isSchade)
        {
            decimal maxDailyKm = 150;
            decimal surchargePerKm = 0.10m;
            decimal dailyRate = 15m;

            decimal basePrice = dailyRate * rentalDuration + kilometersDriven * kilometerPrijs;
            decimal averageDailyKm = kilometersDriven / rentalDuration;
            if (averageDailyKm > maxDailyKm)
            {
                basePrice += (averageDailyKm - maxDailyKm) * surchargePerKm * rentalDuration;
            }
            basePrice *= 0.8m; 
            if (isSchade)
            {
                basePrice += 0m;
            }
            return basePrice;
        }


    }
}