using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DataStructureClasses;
using api.Dtos.KostenDtos;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class KostenRepo : IKostenService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAbonnementService _abonnementService;
        public KostenRepo(ApplicationDbContext context, IAbonnementService abonnementService)
        {
            _context = context;
            _abonnementService = abonnementService;
        }

        public async Task<PrijsOverzichtDto> BerekenVoorschot(int reserveringId, string appuserId)
        {
            var abonnement = await _abonnementService.GetUserAbonnement(appuserId);
            if (abonnement.IsStandaard)
            {
                return await BerekenPayAsYouGo(reserveringId, false);
            }
            if (!abonnement.IsWagenparkAbonnement)
            {
                return await BerekenVoorschotPrijsParticulier(reserveringId, abonnement);
            }
            return await BerekenVoorschotPrijsZakelijk(reserveringId, abonnement);
        }

        public async Task<PrijsOverzichtDto> BerekenDaadWerkelijkPrijs(int reserveringId, int KilometersGereden, bool isSchade, string appuserId)
        {
            var abonnement = await _abonnementService.GetUserAbonnement(appuserId);
            if (abonnement.IsStandaard)
            {
                return await BerekenPayAsYouGo(reserveringId, isSchade);
            }
            if (!abonnement.IsWagenparkAbonnement)
            {
                return await BerekenDaadwerkelijkePrijsParticulier(reserveringId, KilometersGereden , isSchade, abonnement);
            }
            return await BerekenDaadwerkelijkePrijsZakelijk(reserveringId, KilometersGereden, isSchade, abonnement);
        }

        public async Task<PrijsOverzichtDto> BerekenVoorschotPrijsZakelijk(int reserveringId, Abonnement abonnement)
        {
            var reservering = await _context.Reservering
                .FirstOrDefaultAsync(r => r.ReserveringId == reserveringId)
                ?? throw new InvalidOperationException("Reservering not found");

            var voertuigData = await _context.VoertuigData
                .FirstOrDefaultAsync(vs => vs.VoertuigId == reservering.VoertuigId)
                ?? throw new InvalidOperationException("VoertuigData not found");

            var rentalDuration = (reservering.EindDatum - reservering.StartDatum).Days;
            var additionalKilometerCharge = reservering.VerwachtteKM * voertuigData.KilometerPrijs;
            return GetPrijsOpBasisVanAbonnementZakelijk(abonnement, reservering.VerwachtteKM, rentalDuration, voertuigData.KilometerPrijs, false);
        }

        public async Task<PrijsOverzichtDto> BerekenDaadwerkelijkePrijsZakelijk(int reserveringId, decimal kilometersDriven, bool isSchade, Abonnement abonnement)
        {
            var reservering = await _context.Reservering
                .FirstOrDefaultAsync(r => r.ReserveringId == reserveringId)
                ?? throw new InvalidOperationException("Reservering not found");

            var voertuigStatus = await _context.VoertuigData
                .FirstOrDefaultAsync(vs => vs.VoertuigId == reservering.VoertuigId)
                ?? throw new InvalidOperationException("VoertuigData not found");

            var rentalDuration = (reservering.EindDatum - reservering.StartDatum).Days;
            var additionalKilometerCharge = kilometersDriven * voertuigStatus.KilometerPrijs;
            var PrijsOverzicht = GetPrijsOpBasisVanAbonnementZakelijk(abonnement, kilometersDriven, rentalDuration, voertuigStatus.KilometerPrijs, isSchade);

            return PrijsOverzicht;
        }

        public async Task<PrijsOverzichtDto> BerekenVoorschotPrijsParticulier(int reserveringId, Abonnement abonnement)
        {
            var reservering = await _context.Reservering
                .FirstOrDefaultAsync(r => r.ReserveringId == reserveringId)
                ?? throw new InvalidOperationException("Reservering not found");

            var voertuigData = await _context.VoertuigData
                .FirstOrDefaultAsync(v => v.VoertuigId == reservering.VoertuigId)
                ?? throw new InvalidOperationException("VoertuigData not found");

            var rentalDuration = (reservering.EindDatum - reservering.StartDatum).Days;

            return GetPrijsOpBasisVanAbonnementParticulier(
                abonnement,
                reservering.VerwachtteKM,
                rentalDuration,
                voertuigData.KilometerPrijs,
                isSchade: false
            );
        }

        public async Task<PrijsOverzichtDto> BerekenDaadwerkelijkePrijsParticulier(int reserveringId, decimal kilometersDriven, bool isSchade, Abonnement abonnement)
        {
            var reservering = await _context.Reservering
                .FirstOrDefaultAsync(r => r.ReserveringId == reserveringId)
                ?? throw new InvalidOperationException("Reservering not found");

            var voertuigData = await _context.VoertuigData
                .FirstOrDefaultAsync(v => v.VoertuigId == reservering.VoertuigId)
                ?? throw new InvalidOperationException("VoertuigData not found");

            var rentalDuration = (reservering.EindDatum - reservering.StartDatum).Days;

            return GetPrijsOpBasisVanAbonnementParticulier(
                abonnement,
                kilometersDriven,
                rentalDuration,
                voertuigData.KilometerPrijs,
                isSchade
            );
        }

        public async Task <PrijsOverzichtDto> BerekenPayAsYouGo(int reserveringId, bool IsSchade)
        {
            var CurrentReservering = await _context.Reservering.FindAsync(reserveringId) 
                ?? throw new ArgumentException("hier probleem");

            var rentalDuration = (CurrentReservering.EindDatum - CurrentReservering.StartDatum).Days;
            var voertuigData = await _context.VoertuigData.Where(x => x.VoertuigId == CurrentReservering.VoertuigId).FirstOrDefaultAsync()
                ?? throw new ArgumentException("toch hier probleem");
            return ApplyPayAsYouGoPricing(CurrentReservering.VerwachtteKM, rentalDuration, voertuigData.KilometerPrijs, IsSchade);
        }

        public async Task<PrijsOverzichtDto> BerekenVerwachtePrijsUitVerhuurVerzoek(string appuserId, decimal kilometersDriven, DateTime startDatum, DateTime endDatum, int VoertuigId)
        {
            var kilometerPrijs = await _context.VoertuigData.FirstOrDefaultAsync(x => x.VoertuigId == VoertuigId) ?? throw new Exception("geen voertuig gevonoden.");
            var rentalDuration = (endDatum -startDatum).Days;
            var abonnement = await _abonnementService.GetUserAbonnement(appuserId);
            if (abonnement.IsWagenparkAbonnement)
            {
                return GetPrijsOpBasisVanAbonnementZakelijk(abonnement, kilometersDriven, rentalDuration, kilometerPrijs.KilometerPrijs, true);
            }
            return GetPrijsOpBasisVanAbonnementParticulier(abonnement, kilometersDriven, rentalDuration, kilometerPrijs.KilometerPrijs, true);
        }

        private static PrijsOverzichtDto GetPrijsOpBasisVanAbonnementParticulier(Abonnement abonnement, decimal kilometersDriven, int rentalDuration, decimal kilometerPrijs, bool isSchade)
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

        private static PrijsOverzichtDto ApplyPayAsYouGoPricing(decimal kilometersDriven, int rentalDuration, decimal kilometerPrijs, bool isSchade)
        {
            decimal maxDailyKm = 50;
            decimal surchargePerKm = 0.25m;
            decimal dailyRate = 30m;
            decimal basePrice = dailyRate * rentalDuration + kilometersDriven * kilometerPrijs;
            decimal surcharge = 0m;
            decimal damageFee = 0m;
            decimal averageDailyKm = kilometersDriven / rentalDuration;
        
            if (averageDailyKm > maxDailyKm)
            {
                surcharge = (averageDailyKm - maxDailyKm) * surchargePerKm * rentalDuration;
                basePrice += surcharge;
            }
            if (isSchade)
            {
                damageFee = 100m;
                basePrice += damageFee;
            }
        
            var prijsOverzicht = new PrijsOverzichtDto
            {
                TotalePrijs = basePrice,
                PrijsDetails = new List<PrijsOnderdeelDto>
                {
                    new() { Beschrijving = $"Dagkosten = {dailyRate} * {rentalDuration} dagen", Amount = dailyRate * rentalDuration },
                    new() { Beschrijving = $"Kilometer Kosten = {kilometersDriven} km * {kilometerPrijs} per km", Amount = kilometersDriven * kilometerPrijs },
                    new() { Beschrijving = $"Toeslag (overtollige km) = ({averageDailyKm} km/dag - {maxDailyKm} km/dag) * {surchargePerKm} per km * {rentalDuration} dagen", Amount = surcharge },
                    new() { Beschrijving = $"Schadevergoeding = {damageFee} (indien schade)", Amount = damageFee }
                }
            };
        
            return prijsOverzicht;
        }

        private static PrijsOverzichtDto ApplyBasicPricing(decimal kilometersDriven, int rentalDuration, decimal kilometerPrijs, bool isSchade)
        {
            decimal maxDailyKm = 75;
            decimal surchargePerKm = 0.20m;
            decimal dailyRate = 25m;
            decimal basePrice = dailyRate * rentalDuration + kilometersDriven * kilometerPrijs;
            decimal surcharge = 0m;
            decimal damageFee = 0m;
            decimal averageDailyKm = kilometersDriven / rentalDuration;
        
            if (averageDailyKm > maxDailyKm)
            {
                surcharge = (averageDailyKm - maxDailyKm) * surchargePerKm * rentalDuration;
                basePrice += surcharge;
            }
            if (isSchade)
            {
                damageFee = 80m;
                basePrice += damageFee;
            }
        
            var prijsOverzicht = new PrijsOverzichtDto
            {
                TotalePrijs = basePrice,
                PrijsDetails = new List<PrijsOnderdeelDto>
                {
                    new() { Beschrijving = $"Dagkosten = {dailyRate} * {rentalDuration} dagen", Amount = dailyRate * rentalDuration },
                    new() { Beschrijving = $"Kilometer Kosten = {kilometersDriven} km * {kilometerPrijs} per km", Amount = kilometersDriven * kilometerPrijs },
                    new() { Beschrijving = $"Toeslag (overtollige km) = ({averageDailyKm} km/dag - {maxDailyKm} km/dag) * {surchargePerKm} per km * {rentalDuration} dagen", Amount = surcharge },
                    new() { Beschrijving = $"Schadevergoeding = {damageFee} (indien schade)", Amount = damageFee }
                }
            };
        
            return prijsOverzicht;
        }

        private static PrijsOverzichtDto ApplyPremiumPricing(decimal kilometersDriven, int rentalDuration, decimal kilometerPrijs, bool isSchade)
        {
            decimal maxDailyKm = 150;
            decimal surchargePerKm = 0.10m;
            decimal dailyRate = 15m;
            decimal basePrice = dailyRate * rentalDuration + kilometersDriven * kilometerPrijs;
            decimal surcharge = 0m;
            decimal damageFee = 0m;
            decimal averageDailyKm = kilometersDriven / rentalDuration;

            if (averageDailyKm > maxDailyKm)
            {
                surcharge = (averageDailyKm - maxDailyKm) * surchargePerKm * rentalDuration;
                basePrice += surcharge;
            }

            basePrice *= 0.8m; 
            if (isSchade)
            {
                damageFee = 0m; 
            }

            var prijsOverzicht = new PrijsOverzichtDto
            {
                TotalePrijs = basePrice,
                PrijsDetails = new List<PrijsOnderdeelDto>
                {
                    new() { Beschrijving = $"Dagkosten = {dailyRate} * {rentalDuration} dagen", Amount = dailyRate * rentalDuration },
                    new() { Beschrijving = $"Kilometer Kosten = {kilometersDriven} km * {kilometerPrijs} per km", Amount = kilometersDriven * kilometerPrijs },
                    new() { Beschrijving = $"Toeslag (overtollige km) = ({averageDailyKm} km/dag - {maxDailyKm} km/dag) * {surchargePerKm} per km * {rentalDuration} dagen", Amount = surcharge },
                    new() { Beschrijving = $"Korting voor Premium = -({basePrice * 0.2m})", Amount = -(basePrice * 0.2m) },
                    new() { Beschrijving = $"Schadevergoeding = {damageFee} (indien schade)", Amount = damageFee }
                }
            };

            return prijsOverzicht;
        }

        private static PrijsOverzichtDto ApplyProfessionalPricing(decimal kilometersDriven, int rentalDuration, decimal kilometerPrijs, bool isSchade)
        {
            decimal maxDailyKm = 100;
            decimal surchargePerKm = 0.15m;
            decimal dailyRate = 20m;
            decimal basePrice = dailyRate * rentalDuration + kilometersDriven * kilometerPrijs;
            decimal surcharge = 0m;
            decimal damageFee = 0m;
            decimal averageDailyKm = kilometersDriven / rentalDuration;
            if (averageDailyKm > maxDailyKm)
            {
                surcharge = (averageDailyKm - maxDailyKm) * surchargePerKm * rentalDuration;
                basePrice += surcharge;
            }
            basePrice *= 0.9m; 
            if (isSchade)
            {
                damageFee = 50m; 
                basePrice += damageFee;
            }
            var prijsOverzicht = new PrijsOverzichtDto
            {
                TotalePrijs = basePrice,
                PrijsDetails = new List<PrijsOnderdeelDto>
                {
                    new() { Beschrijving = $"Dagkosten = {dailyRate} * {rentalDuration} dagen", Amount = dailyRate * rentalDuration },
                    new() { Beschrijving = $"Kilometer Kosten = {kilometersDriven} km * {kilometerPrijs} per km", Amount = kilometersDriven * kilometerPrijs },
                    new() { Beschrijving = $"Toeslag (overtollige km) = ({averageDailyKm} km/dag - {maxDailyKm} km/dag) * {surchargePerKm} per km * {rentalDuration} dagen", Amount = surcharge },
                    new() { Beschrijving = $"Korting voor Professioneel = -({basePrice * 0.1m})", Amount = -(basePrice * 0.1m) },
                    new() { Beschrijving = $"Schadevergoeding = {damageFee} (indien schade)", Amount = damageFee }
                }
            };

            return prijsOverzicht;
        }

        private static PrijsOverzichtDto GetPrijsOpBasisVanAbonnementZakelijk(Abonnement abonnement, decimal kilometersDriven, int rentalDuration, decimal kilometerPrijs, bool isSchade)
        {
            return abonnement.Naam switch
            {
                AbonnementNamen.WagenparkBasic => ApplyWagenparkBasicPricing(kilometersDriven, rentalDuration, kilometerPrijs, isSchade),
                AbonnementNamen.WagenparkPremium => ApplyWagenparkPremiumPricing(kilometersDriven, rentalDuration, kilometerPrijs, isSchade),
                _ => throw new InvalidOperationException("Unknown business abonnement type")
            };
        }

        private static PrijsOverzichtDto ApplyWagenparkBasicPricing(decimal kilometersDriven, int rentalDuration, decimal kilometerPrijs, bool isSchade)
        {
            decimal maxDailyKm = 100;
            decimal surchargePerKm = 0.18m;
            decimal dailyRate = 50m;
            decimal basePrice = dailyRate * rentalDuration + kilometersDriven * kilometerPrijs;
            decimal surcharge = 0m;
            decimal damageFee = 0m;
            decimal averageDailyKm = kilometersDriven / rentalDuration;

            if (averageDailyKm > maxDailyKm)
            {
                surcharge = (averageDailyKm - maxDailyKm) * surchargePerKm * rentalDuration;
                basePrice += surcharge;
            }

            if (isSchade)
            {
                damageFee = 150m; 
                basePrice += damageFee;
            }

            var prijsOverzicht = new PrijsOverzichtDto
            {
                TotalePrijs = basePrice,
                PrijsDetails = new List<PrijsOnderdeelDto>
                {
                    new() { Beschrijving = $"Dagkosten = {dailyRate} * {rentalDuration} dagen", Amount = dailyRate * rentalDuration },
                    new() { Beschrijving = $"Kilometer Kosten = {kilometersDriven} km * {kilometerPrijs} per km", Amount = kilometersDriven * kilometerPrijs },
                    new() { Beschrijving = $"Toeslag (overtollige km) = ({averageDailyKm} km/dag - {maxDailyKm} km/dag) * {surchargePerKm} per km * {rentalDuration} dagen", Amount = surcharge },
                    new() { Beschrijving = $"Schadevergoeding = {damageFee} (indien schade)", Amount = damageFee }
                }
            };

            return prijsOverzicht;
        }

        private static PrijsOverzichtDto ApplyWagenparkPremiumPricing(decimal kilometersDriven, int rentalDuration, decimal kilometerPrijs, bool isSchade)
        {
            decimal maxDailyKm = 150;
            decimal surchargePerKm = 0.10m;
            decimal dailyRate = 75m;
            decimal basePrice = dailyRate * rentalDuration + kilometersDriven * kilometerPrijs;
            decimal surcharge = 0m;
            decimal damageFee = 0m;
            decimal averageDailyKm = kilometersDriven / rentalDuration;
            if (averageDailyKm > maxDailyKm)
            {
                surcharge = (averageDailyKm - maxDailyKm) * surchargePerKm * rentalDuration;
                basePrice += surcharge;
            }
            basePrice *= 0.85m; 
            if (isSchade)
            {
                damageFee = 200m; 
                basePrice += damageFee;
            }
            var prijsOverzicht = new PrijsOverzichtDto
            {
                TotalePrijs = basePrice,
                PrijsDetails = new List<PrijsOnderdeelDto>
                {
                    new() { Beschrijving = $"Dagkosten = {dailyRate} * {rentalDuration} dagen", Amount = dailyRate * rentalDuration },
                    new() { Beschrijving = $"Kilometer Kosten = {kilometersDriven} km * {kilometerPrijs} per km", Amount = kilometersDriven * kilometerPrijs },
                    new() { Beschrijving = $"Toeslag (overtollige km) = ({averageDailyKm} km/dag - {maxDailyKm} km/dag) * {surchargePerKm} per km * {rentalDuration} dagen", Amount = surcharge },
                    new() { Beschrijving = $"Korting voor Premium = -({basePrice * 0.15m})", Amount = -(basePrice * 0.15m) },
                    new() { Beschrijving = $"Schadevergoeding = {damageFee} (indien schade)", Amount = damageFee }
                }
            };
            return prijsOverzicht;
        }


    }
}