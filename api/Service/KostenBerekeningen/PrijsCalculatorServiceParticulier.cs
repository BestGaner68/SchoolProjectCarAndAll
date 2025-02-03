using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DataStructureClasses;
using api.Dtos.KostenDtos;
using api.Models;

namespace api.Service.KostenBerekeningen
{
    public class PrijsCalculatorServiceParticulier
    {
        public PrijsOverzichtDto Bereken(Reservering reservering, Abonnement abonnement, decimal kilometerPrijs, decimal kilometersGereden)
        {
            return abonnement.Naam switch
            {
                AbonnementNamen.PayAsYouGo => BerekenPayAsYouGo(reservering, kilometerPrijs, kilometersGereden),
                AbonnementNamen.Basic => BerekenBasic(reservering, kilometerPrijs, kilometersGereden),
                AbonnementNamen.Professional => BerekenProfessional(reservering, kilometerPrijs, kilometersGereden),
                AbonnementNamen.Premium => BerekenPremium(reservering, kilometerPrijs, kilometersGereden),
                _ => throw new InvalidOperationException("Onbekend abonnement type")
            };
        }

        private PrijsOverzichtDto BerekenPayAsYouGo(Reservering reservering, decimal kilometerPrijs, decimal kilometersGereden)
        {
            return BerekenPrijs(reservering, kilometerPrijs, kilometersGereden, 50, 0.25m, 30m, 0m);
        }

        private PrijsOverzichtDto BerekenBasic(Reservering reservering, decimal kilometerPrijs, decimal kilometersGereden)
        {
            return BerekenPrijs(reservering, kilometerPrijs, kilometersGereden, 75, 0.20m, 25m, 0m);
        }

        private PrijsOverzichtDto BerekenProfessional(Reservering reservering, decimal kilometerPrijs, decimal kilometersGereden)
        {
            return BerekenPrijs(reservering, kilometerPrijs, kilometersGereden, 100, 0.15m, 20m, 0.1m);
        }

        private PrijsOverzichtDto BerekenPremium(Reservering reservering, decimal kilometerPrijs, decimal kilometersGereden)
        {
            return BerekenPrijs(reservering, kilometerPrijs, kilometersGereden, 150, 0.10m, 15m, 0.2m);
        }

        private PrijsOverzichtDto BerekenPrijs(Reservering reservering, decimal kilometerPrijs, decimal kilometersGereden, decimal maxDagKm, decimal toeslagPerKm, decimal dagTarief, decimal korting)
        {
            int rentalDuration = (reservering.EindDatum - reservering.StartDatum).Days;
            decimal basePrice = dagTarief * rentalDuration + kilometersGereden * kilometerPrijs;
            decimal surcharge = BerekenToeslag(kilometersGereden, rentalDuration, maxDagKm, toeslagPerKm);
            decimal discount = basePrice * korting;

            basePrice += surcharge - discount;

            return new PrijsOverzichtDto
            {
                TotalePrijs = basePrice,
                PrijsDetails = new List<PrijsOnderdeelDto>
                {
                    new() { Beschrijving = $"Dagkosten = {dagTarief} * {rentalDuration} dagen", Amount = dagTarief * rentalDuration },
                    new() { Beschrijving = $"Kilometer Kosten = {kilometersGereden} km * {kilometerPrijs} per km", Amount = kilometersGereden * kilometerPrijs },
                    new() { Beschrijving = $"Toeslag (overtollige km)", Amount = surcharge },
                    new() { Beschrijving = $"Korting op abonnement", Amount = -discount }
                }
            };
        }

        private decimal BerekenToeslag(decimal kilometersGereden, int rentalDuration, decimal maxDagKm, decimal toeslagPerKm)
        {
            decimal gemiddeldeKmPerDag = kilometersGereden / rentalDuration;
            return gemiddeldeKmPerDag > maxDagKm
                ? (gemiddeldeKmPerDag - maxDagKm) * toeslagPerKm * rentalDuration
                : 0m;
        }
    }


}