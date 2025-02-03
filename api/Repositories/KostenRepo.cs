using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.KostenDtos;
using api.Interfaces;
using api.Mapper;
using api.Service.KostenBerekeningen;

namespace api.Repositories
{
    public class KostenService : IKostenService
    {
        private readonly IReserveringService _reserveringRepo;
        private readonly PrijsCalculatorServiceParticulier _prijsCalculatorParticulier;
        private readonly PrijsCalculatorZakelijkService _prijsCalculatorZakelijk;
        private readonly AccessoiresPrijsService _accessoirePrijsService;
        private readonly SchadePrijsService _schadePrijsService;
        private readonly IAbonnementService _abonnementService;
        private readonly IVerhuurVerzoekService _verhuurVerzoekService;
    
        public KostenService(
            IReserveringService reserveringRepo,
            PrijsCalculatorServiceParticulier prijsCalculator,
            AccessoiresPrijsService accessoirePrijsService,
            SchadePrijsService schadePrijsService,
            IAbonnementService abonnementService,
            PrijsCalculatorZakelijkService prijsCalculatorZakelijk,
            IVerhuurVerzoekService verhuurVerzoekService)
        {
            _reserveringRepo = reserveringRepo;
            _prijsCalculatorParticulier = prijsCalculator;
            _accessoirePrijsService = accessoirePrijsService;
            _schadePrijsService = schadePrijsService;
            _abonnementService = abonnementService;
            _prijsCalculatorZakelijk = prijsCalculatorZakelijk;
            _verhuurVerzoekService = verhuurVerzoekService;
        }
    
        public async Task<PrijsOverzichtDto> BerekenTotalePrijs(int reserveringId, bool isSchade, decimal kilometersGereden)
        {
            var reservering = await _reserveringRepo.GetById(reserveringId);
            var abonnement = await _abonnementService.GetUserAbonnement(reservering.AppUserId);
            var accessoiresKosten = await _accessoirePrijsService.Bereken(reserveringId);
            var schadeKosten = _schadePrijsService.Bereken(isSchade, reservering.Verzekering);
            var kilometerPrijs = await _reserveringRepo.GetKilometerPrijs(reservering.VoertuigId);

            var basisPrijs = abonnement.IsWagenparkAbonnement
                ? _prijsCalculatorZakelijk.Bereken(reservering, abonnement, kilometerPrijs, kilometersGereden)
                : _prijsCalculatorParticulier.Bereken(reservering, abonnement, kilometerPrijs, kilometersGereden);

            basisPrijs.TotalePrijs += accessoiresKosten.TotalePrijs + schadeKosten.TotalePrijs;
            basisPrijs.PrijsDetails.AddRange(accessoiresKosten.PrijsDetails);
            basisPrijs.PrijsDetails.AddRange(schadeKosten.PrijsDetails);

            return basisPrijs;
        }


        public async Task<PrijsOverzichtDto> BerekenVerwachtePrijsUitVerhuurVerzoek(int VerhuurverzoekId)
        {
            bool isSchade = true;
            var Verhuurverzoek = await _verhuurVerzoekService.GetByIdAsync(VerhuurverzoekId) ?? throw new Exception("Geen reservering gevonden");
            var CurrentReservering = VerhuurVerzoekMapper.ToReserveringFromVerhuurVerzoek(Verhuurverzoek);
            var abonnement = await _abonnementService.GetUserAbonnement(CurrentReservering.AppUserId);
            var accessoiresKosten = await _accessoirePrijsService.Bereken(CurrentReservering.ReserveringId);
            var schadeKosten = _schadePrijsService.Bereken(isSchade, CurrentReservering.Verzekering);
            var kilometerPrijs = await _reserveringRepo.GetKilometerPrijs(CurrentReservering.VoertuigId);
            

            var basisPrijs = abonnement.IsWagenparkAbonnement
                ? _prijsCalculatorZakelijk.Bereken(CurrentReservering, abonnement, kilometerPrijs, CurrentReservering.VerwachtteKM)
                : _prijsCalculatorParticulier.Bereken(CurrentReservering, abonnement, kilometerPrijs, CurrentReservering.VerwachtteKM);

            basisPrijs.TotalePrijs += accessoiresKosten.TotalePrijs + schadeKosten.TotalePrijs;
            basisPrijs.PrijsDetails.AddRange(accessoiresKosten.PrijsDetails);
            basisPrijs.PrijsDetails.AddRange(schadeKosten.PrijsDetails);

            return basisPrijs;
                
        }
    }
}