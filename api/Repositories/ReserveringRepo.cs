using api.Data;
using api.Interfaces;
using api.Mapper;
using api.Models;
using Microsoft.EntityFrameworkCore;
using api.Dtos.ReserveringenEnSchade;
using api.DataStructureClasses;

namespace api.Repositories
{
    public class ReserveringRepo : IReserveringService
    {
        private readonly ApplicationDbContext _context;
        private readonly IVoertuigService _voertuigService;
        private readonly IWagenparkService _wagenparkService;
        private readonly IWagenParkUserListService _wagenparkUserListService;
        public ReserveringRepo (ApplicationDbContext context, IVoertuigService voertuigService, IWagenparkService wagenparkService, IWagenParkUserListService wagenParkUserListService){
            _context = context;
            _voertuigService = voertuigService;
            _wagenparkService = wagenparkService;
            _wagenparkUserListService = wagenParkUserListService;
        }
        
        public async Task<bool> AcceptVerhuurVerzoek(int verhuurVerzoekId)
        {
            try
            {
                var CurrentVerhuurVerzoek = await _context.VerhuurVerzoek.FindAsync(verhuurVerzoekId);
                if (CurrentVerhuurVerzoek == null){
                    return false;
                }
                CurrentVerhuurVerzoek.Status = "Geaccpeteerd";
                var CurrentReservering = VerhuurVerzoekMapper.ToReserveringFromVerhuurVerzoek(CurrentVerhuurVerzoek);
                var UserWagenPark = await _wagenparkUserListService.GetWagenParkByAppUserId(CurrentReservering.AppUserId);
                if (!(UserWagenPark == null))
                {
                    CurrentReservering.Type = "Zakelijk";
                }
                else
                {
                    CurrentReservering.Type = "Particulier";
                }
                var User = await _context.Users.FindAsync(CurrentReservering.AppUserId);
                CurrentReservering.Fullname = $"{User.Voornaam} {User.Achternaam}";
                await _context.Reservering.AddAsync(CurrentReservering);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false; 
            }
        }

        public async Task<bool> WeigerVerhuurVerzoek(int verhuurVerzoekId)
        {
            try{
                var CurrentVerhuurVerzoek = await _context.VerhuurVerzoek.FindAsync(verhuurVerzoekId);
                if (CurrentVerhuurVerzoek == null) 
                {
                    return false;
                }
                CurrentVerhuurVerzoek.Status = "Geweigerd";
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Reservering>> GetAll()
        {
           var result = await _context.Reservering.ToListAsync();
           return result;
        }

        public async Task<Reservering> GetById(int ReserveringId)
        {
            var reservering = await _context.Reservering.FindAsync(ReserveringId);
            if (reservering == null)
            {
                return null;
            }
            return reservering;
        }

        public async Task<bool> GeefUit(int ReserveringId)
        {
            Console.WriteLine($"Ontvangen ReserveringId: {ReserveringId}");
            var CurrentReservering = await _context.Reservering.FindAsync(ReserveringId);
            if (CurrentReservering==null)
            {
                Console.WriteLine(CurrentReservering);
                return false;
            }
            var voertuig = await _context.VoertuigStatus.FindAsync(CurrentReservering.VoertuigId);
            if (voertuig == null){
                Console.WriteLine($"{CurrentReservering}  {voertuig}");
                return false;
            }
            Console.WriteLine("Alles ok");
            voertuig.Status = VoertuigStatussen.Uitgegeven;
            CurrentReservering.Status = "Uitgegeven";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> NeemIn(int ReserveringId)
        {
            var CurrentReservering = await _context.Reservering.FindAsync(ReserveringId);
            if (CurrentReservering==null)
            {
                return false;
            }
            var voertuig = await _context.VoertuigStatus.FindAsync(CurrentReservering.VoertuigId);
            if (voertuig == null){
                return false;
            }
            voertuig.Status = VoertuigStatussen.KlaarVoorGebruik;
            CurrentReservering.Status = "Afgerond";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MeldSchadeVanuitReservering(int ReserveringId, string Schade, IFormFile? schadeFoto)
        {
            var CurrentReservering = await _context.Reservering.FindAsync(ReserveringId);
            if (CurrentReservering == null){
                return false;
            }
            var schadeformulier = new SchadeFormulier{
                VoertuigId = CurrentReservering.VoertuigId,
                Schade = Schade,
                SchadeDatum = DateTime.Now
            };

            if (schadeFoto != null)
            {
                using var memoryStream = new MemoryStream();
                await schadeFoto.CopyToAsync(memoryStream);
                schadeformulier.SchadeFoto = memoryStream.ToArray(); 
            }
            
            var voertuig = await _context.VoertuigStatus.FindAsync(CurrentReservering.VoertuigId);
            if (voertuig == null){
                return false;
            }
            voertuig.Status = VoertuigStatussen.SchadeGemeld;
            await _context.SchadeFormulier.AddAsync(schadeformulier);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Reservering>> GetMyReserveringen(string AppUserId){
            return await _context.Reservering
                .Where(r => r.AppUserId == AppUserId)
                .ToListAsync();    
        }

        public async Task<HuurGeschiedenisDto> GetHuurGeschiedenis(Reservering reservering)
        {
            var voertuigData = await _voertuigService.GetAllVoertuigDataById(reservering.VoertuigId);
            return ReserveringMapper.ToHuurGeschiedenisDto(reservering, voertuigData);
        }
        public async Task<bool> MeldSchadeVanuitVoertuigId(int VoertuigId, string Schade, IFormFile? Foto)
        {
            var huidigeVoertuig = await _context.Voertuig.FindAsync(VoertuigId);
            if (huidigeVoertuig == null)
            {
                return false; 
            }
            var schadeformulier = new SchadeFormulier
            {
                VoertuigId = VoertuigId,
                Schade = Schade,
                SchadeDatum = DateTime.Now,
                Status = SchadeStatus.Ingediend 
            };
            if (Foto != null)
            {
                using var memoryStream = new MemoryStream();
                await Foto.CopyToAsync(memoryStream);
                schadeformulier.SchadeFoto = memoryStream.ToArray();
            }
            _context.SchadeFormulier.Add(schadeformulier);
            await _context.SaveChangesAsync();

            return true; 
        }
    }
}