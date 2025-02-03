using api.Data;
using api.Interfaces;
using api.Mapper;
using api.Models;
using Microsoft.EntityFrameworkCore;
using api.Dtos.ReserveringenEnSchade;
using api.DataStructureClasses;
using api.Dtos.Verhuur;

namespace api.Repositories
{
    public class ReserveringRepo : IReserveringService
    {
        private readonly ApplicationDbContext _context;
        private readonly IVoertuigService _voertuigService;
        private readonly IWagenparkService _wagenparkService;
        private readonly IWagenParkUserListService _wagenparkUserListService;
        private readonly IEmailService _emailService;
        public ReserveringRepo (ApplicationDbContext context, IVoertuigService voertuigService, IWagenparkService wagenparkService, IWagenParkUserListService wagenParkUserListService, IEmailService email){
            _context = context;
            _voertuigService = voertuigService;
            _wagenparkService = wagenparkService;
            _wagenparkUserListService = wagenParkUserListService;
            _emailService = email;
        }
        
        public async Task<bool> AcceptVerhuurVerzoek(int verhuurVerzoekId)
        {       
            var CurrentVerhuurVerzoek = await _context.VerhuurVerzoek.FindAsync(verhuurVerzoekId);
            if (CurrentVerhuurVerzoek == null)
            {
                return false;
            }
            CurrentVerhuurVerzoek.Status = VerhuurVerzoekStatussen.Geaccepteerd;
            var CurrentReservering = VerhuurVerzoekMapper.ToReserveringFromVerhuurVerzoek(CurrentVerhuurVerzoek);
            var User = await _context.Users.FindAsync(CurrentReservering.AppUserId);
            CurrentReservering.Fullname = $"{User.Voornaam} {User.Achternaam}";
            if (CurrentReservering.StartDatum > DateTime.UtcNow.AddDays(7))
            {
                CurrentReservering.Status = ReserveringStatussen.MagWordenGewijzigd;
            }
            else
            {
                CurrentReservering.Status = ReserveringStatussen.ReadyForPickUp;
            }
            await _context.Reservering.AddAsync(CurrentReservering);
            await _context.SaveChangesAsync();
            var Voertuig = await _context.Voertuig.FindAsync(CurrentVerhuurVerzoek.VoertuigId);
            var emailMetadata = new EmailMetaData
            {
                ToAddress = User.Email,
                Subject = "Uw verhuurverzoek is Geaccepteerd",
                Body = EmailTemplates.RentalRequestAccepted(CurrentReservering, Voertuig)
            };

            // Send the email
            await _emailService.SendEmail(emailMetadata);
            return true;
        }       
        public async Task<bool> WeigerVerhuurVerzoek(WeigerVerhuurVerzoekDto weigerVerhuurVerzoekDto)
        {
            try{
                var CurrentVerhuurVerzoek = await _context.VerhuurVerzoek.FindAsync(weigerVerhuurVerzoekDto.VerhuurverzoekId);
                if (CurrentVerhuurVerzoek == null) 
                {
                    return false;
                }
                CurrentVerhuurVerzoek.Status = VerhuurVerzoekStatussen.Geweigerd;
                await _context.SaveChangesAsync();
                var user = await _context.Users.FindAsync(CurrentVerhuurVerzoek.AppUserId);
                var emailMetadata = new EmailMetaData
                {
                    ToAddress = user.Email,
                    Subject = "Uw verhuurverzoek is geweigerd",
                    Body = EmailTemplates.RentalRequestRejected(user, weigerVerhuurVerzoekDto.RedenVoorWeigering)
                };

                // Send the email
                await _emailService.SendEmail(emailMetadata);
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
        

        public async Task<bool> VerwijderReservering(int reserveringId)
        {   
            var reservering = await _context.Reservering.FindAsync(reserveringId) 
                ?? throw new Exception("reservering niet gevonden");

            if (reservering.Status == ReserveringStatussen.MagWordenGewijzigd)
            {
                _context.Reservering.Remove(reservering);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new Exception("Reservering kan niet meer worden verwijderd");
        }

        public async Task<bool> WijzigReservering(WijzigReserveringDto wijzigReserveringDto)
        {
            {
                var reservering = await _context.Reservering.FindAsync(wijzigReserveringDto.ReserveringId);
                if (reservering == null)
                {
                    return false; 
                }
                if (reservering.Status == "MagWordenGewijzigd")
                {

                    if (wijzigReserveringDto.NewStartDatum < DateTime.UtcNow || wijzigReserveringDto.NewEindDatum < DateTime.UtcNow)
                    {
                        throw new Exception("De ingevulde Data zijn al geweest."); 
                    }
                    if (wijzigReserveringDto.NewStartDatum.HasValue && wijzigReserveringDto.NewEindDatum.HasValue)
                    {
                        reservering.StartDatum = wijzigReserveringDto.NewStartDatum.Value;
                        reservering.EindDatum = wijzigReserveringDto.NewEindDatum.Value;
                    }

                    if (wijzigReserveringDto.NieuwVoertuigId.HasValue)
                    {
                        reservering.VoertuigId = wijzigReserveringDto.NieuwVoertuigId.Value;
                    }

                    await _context.SaveChangesAsync();
                    return true;
                }
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
            var reservering = await _context.Reservering.FindAsync(ReserveringId) ?? throw new ArgumentException($"Geen Reservering gevonden met Id {ReserveringId}");
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
            var voertuig = await _context.VoertuigData.FirstOrDefaultAsync(x => x.VoertuigId == CurrentReservering.VoertuigId);
            if (voertuig == null){
                Console.WriteLine($"{CurrentReservering}  {voertuig}");
                return false;
            }
            Console.WriteLine("Alles ok");
            voertuig.Status = VoertuigStatussen.Uitgegeven;
            CurrentReservering.Status = ReserveringStatussen.Uitgegeven;
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
            var voertuig = await _context.VoertuigData.FindAsync(CurrentReservering.VoertuigId);
            if (voertuig == null){
                return false;
            }
            voertuig.Status = VoertuigStatussen.KlaarVoorGebruik;
            CurrentReservering.Status = ReserveringStatussen.Afgerond;
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
            
            var voertuig = await _context.VoertuigData.FirstOrDefaultAsync(x => x.VoertuigId == CurrentReservering.VoertuigId);
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
            var voertuigData = await _voertuigService.GetSimpleVoertuigDataById(reservering.VoertuigId);
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

        public async Task<Reservering?> GetReserveringById(int reserveringId)
        {
            return await _context.Reservering.FindAsync(reserveringId) ?? null;
        }

        public async Task<Reservering> GetByIdOverzicht(int ReserveringId)
        {
            return await _context.Reservering.Include(x => x.Verzekering)
                .Include(x => x.Accessoires)
                .FirstOrDefaultAsync(x => x.ReserveringId == ReserveringId)
                ?? throw new ArgumentException("Geen OverzichtData Gevonden")
                ;
        }

        public async Task<decimal> GetKilometerPrijs(int voertuigId)
        {
            var voertuigData = await _context.VoertuigData
                .Where(vd => vd.VoertuigId == voertuigId)
                .Select(vd => vd.KilometerPrijs)
                .FirstOrDefaultAsync();

        if (voertuigData == 0)
            throw new InvalidOperationException("Kilometerprijs niet gevonden voor dit voertuig");

        return voertuigData;
        }
    }
}