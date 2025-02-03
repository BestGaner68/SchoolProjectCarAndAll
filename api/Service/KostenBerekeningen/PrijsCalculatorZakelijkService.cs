using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DataStructureClasses;
using api.Dtos.KostenDtos;
using api.Models;

namespace api.Service.KostenBerekeningen
{
    public class PrijsCalculatorZakelijkService
    {
        public PrijsOverzichtDto Bereken(Reservering reservering, Abonnement abonnement, decimal kilometerPrijs, decimal kilometersGereden)
        {
            return abonnement.Naam switch
            {
                AbonnementNamen.WagenparkBasic => BerekenWagenparkBasic(reservering, kilometerPrijs, kilometersGereden),
                AbonnementNamen.WagenparkPremium => BerekenWagenparkPremium(reservering, kilometerPrijs, kilometersGereden),
                _ => throw new InvalidOperationException("Onbekend zakelijk abonnement type")
            };
        }

        private PrijsOverzichtDto BerekenWagenparkBasic(Reservering reservering, decimal kilometerPrijs, decimal kilometersGereden)
        {
            return BerekenZakelijkePrijs(reservering, kilometerPrijs, kilometersGereden, 100, 0.18m, 50m, 0.1m);
        }

        private PrijsOverzichtDto BerekenWagenparkPremium(Reservering reservering, decimal kilometerPrijs, decimal kilometersGereden)
        {
            return BerekenZakelijkePrijs(reservering, kilometerPrijs, kilometersGereden, 150, 0.10m, 75m, 0.15m);
        }

        private PrijsOverzichtDto BerekenZakelijkePrijs(Reservering reservering, decimal kilometerPrijs, decimal kilometersGereden, decimal maxDagKm, decimal toeslagPerKm, decimal dagTarief, decimal korting)
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
                    new() { Beschrijving = $"Zakelijke korting", Amount = -discount }
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