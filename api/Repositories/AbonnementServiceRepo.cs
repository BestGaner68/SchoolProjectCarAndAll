using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DataStructureClasses;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class AbonnementServiceRepo :IAbonnementService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        public AbonnementServiceRepo(ApplicationDbContext context, IWagenparkService wagenparkService, IEmailService emailService, UserManager<AppUser> userManager)
        {
            _context=context;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<bool> ExtentCurrentAbonnement(int wagenParkId)
        {

            var abonnement = await _context.WagenparkAbonnementen
                .FirstOrDefaultAsync(wa => wa.WagenParkId == wagenParkId && wa.IsActief == true);
            if (abonnement == null)
            {
                return false;
            }
            if (!abonnement.EindDatum.HasValue)
            {
                throw new ArgumentException("Kan het standaardAbonnement niet verlengen.");
            }
            abonnement.EindDatum = abonnement.EindDatum.Value.AddMonths(3);
            await _context.SaveChangesAsync();
            return true; 
        }

        public async Task<bool> GeefStandaardAbonnement(AppUser appUser)
        {
            var abonnement = await _context.Abonnementen
                .FirstOrDefaultAsync(a => a.Naam == "PayAsYouGo") 
                ?? throw new InvalidOperationException("Default abonnement 'PayAsYouGo' not found.");

           
            var userAbonnement = new UserAbonnement
            {
                AppUserId = appUser.Id,
                AbonnementId = abonnement.AbonnementId,
                StartDate = DateTime.UtcNow, 
                EndDate = null
            };

            _context.UserAbonnementen.Add(userAbonnement);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Abonnement> GetActiveWagenparkAbonnement(int wagenparkId)
        {
            var wagenpark = await _context.Wagenpark
                .Include(wp => wp.WagenparkAbonnementen)
                .ThenInclude(wa => wa.Abonnement)
                .FirstOrDefaultAsync(wp => wp.WagenParkId == wagenparkId) ?? throw new Exception("Gebruiker is geen eigenaar van een wagenpark");
            var activeAbonnement = wagenpark.WagenparkAbonnementen
                .FirstOrDefault(wa => wa.IsActief);
  
            return activeAbonnement?.Abonnement;
        }

        public async Task<IEnumerable<Abonnement>> GetAllUserAbonnementen()
        {
            return await _context.Abonnementen.Where(x => !x.IsWagenparkAbonnement).ToListAsync();
        }

        public async Task<IEnumerable<Abonnement>> GetAllWagenparkBeheerderAbonnementen()
        {
            return await _context.Abonnementen.Where(x => x.IsWagenparkAbonnement).ToListAsync();
        }

        public async Task<Abonnement> GetUserAbonnement(string appUserId)
        {
            var currentUser = await _context.Users.FindAsync(appUserId) 
                ?? throw new ArgumentException("Abonnement not found.");
        

            var roles = await _userManager.GetRolesAsync(currentUser);
            if (roles.Contains(Rollen.ParticuliereKlant))
            {
                var abonnementId = await _context.UserAbonnementen
                    .Where(w => w.AppUserId == appUserId && w.IsActive)
                    .Select(w => w.AbonnementId)
                    .FirstOrDefaultAsync();

                if (abonnementId == 0) throw new ArgumentException("");
                var UserAbonnement = await _context.Abonnementen.FindAsync(abonnementId);
                return UserAbonnement ?? throw new ArgumentException("");
            }
            else
            {
                var userWagenParkId = await _context.WagenParkUserLists
                    .Where(w => w.AppUserId == appUserId)
                    .Select(w => w.WagenParkId)
                    .FirstOrDefaultAsync();

                if (userWagenParkId == 0) throw new ArgumentException("");
                var userWagenpark = await _context.Wagenpark.FindAsync(userWagenParkId) 
                    ?? throw new ArgumentException("");

                var abonnement = await _context.WagenparkAbonnementen
                    .Where(a => a.WagenParkId == userWagenpark.WagenParkId && a.IsActief == true)
                    .FirstOrDefaultAsync() ?? throw new ArgumentException("");
            
                var UserAbonnement = await _context.Abonnementen.FindAsync(abonnement.AbonnementId) 
                    ?? throw new ArgumentException("");
                
                return UserAbonnement;
            }
        }



        public async Task<bool> WijzigAbonnementWagenpark(int wagenParkId, int nieuwAbonnementId)
        {
            // Haal het wagenpark op met abonnementen en de gekoppelde gebruiker
            var currentWagenPark = await _context.Wagenpark
                .Include(wp => wp.WagenparkAbonnementen)
                .Include(wp => wp.AppUser)
                .FirstOrDefaultAsync(wp => wp.WagenParkId == wagenParkId);

            if (currentWagenPark == null)
                throw new ArgumentException("Geen wagenpark gevonden.");

            if (currentWagenPark.WagenparkAbonnementen == null)
                throw new NullReferenceException("Wagenpark heeft geen abonnementen.");

            // Haal het nieuwe abonnement op en controleer of het geldig is
            var nieuwAbonnement = await _context.Abonnementen
                .FirstOrDefaultAsync(a => a.AbonnementId == nieuwAbonnementId && !a.IsStandaard && a.IsWagenparkAbonnement);

            if (nieuwAbonnement == null)
                throw new ArgumentException($"Geen geldig abonnement gevonden met ID {nieuwAbonnementId}.");

            // Zoek het huidige actieve abonnement
            var huidigAbonnementLine = currentWagenPark.WagenparkAbonnementen
                .FirstOrDefault(wa => wa.IsActief)
                ?? throw new Exception("Geen actief abonnement gevonden.");

            var huidigAbonnement = await _context.Abonnementen.FindAsync(huidigAbonnementLine.AbonnementId);

            if (huidigAbonnement == null)
                throw new ArgumentException("Huidig abonnement niet gevonden in de database.");

            var tempStartDatum = huidigAbonnementLine.EindDatum ?? DateTime.UtcNow;

            // Controleer of het nieuwe abonnement hetzelfde is als het lopende abonnement
            if (huidigAbonnementLine.AbonnementId == nieuwAbonnement.AbonnementId)
                throw new InvalidOperationException("Nieuw abonnement kan niet hetzelfde zijn als het lopende abonnement.");
            if (huidigAbonnement.IsStandaard)
            {
                huidigAbonnementLine.IsActief = false;

                currentWagenPark.WagenparkAbonnementen.Add(new WagenparkAbonnementen
                {
                    Abonnement = nieuwAbonnement,
                    StartDatum = DateTime.UtcNow,
                    EindDatum = DateTime.UtcNow.AddDays(90),
                    IsActief = true
                });

                _context.Remove(huidigAbonnement);
            }
            else
            {
                currentWagenPark.WagenparkAbonnementen.Add(new WagenparkAbonnementen
                {
                    Abonnement = nieuwAbonnement,
                    StartDatum = tempStartDatum,
                    EindDatum = tempStartDatum.AddDays(90),
                    IsActief = false
                });
            }

            // Controleer of de gebruiker een e-mailadres heeft voordat een e-mail wordt verzonden
            if (string.IsNullOrEmpty(currentWagenPark.AppUser?.Email))
                throw new NullReferenceException("AppUser of Email is niet ingesteld voor dit wagenpark.");

            var emailSubject = huidigAbonnement.IsStandaard
                ? "Abonnement Wijziging Bevestigd"
                : "Abonnement Wijziging Gepland";

            var emailBody = huidigAbonnement.IsStandaard
                ? EmailTemplates.GetAbonnementWijzigingBevestigdBody(nieuwAbonnement.Naam)
                : EmailTemplates.GetAbonnementWijzigingGeplandBody(nieuwAbonnement.Naam, tempStartDatum);

            var emailMetaData = new EmailMetaData
            {
                ToAddress = currentWagenPark.AppUser.Email,
                Subject = emailSubject,
                Body = emailBody
            };

            // Veilige e-mailverzending met foutafhandeling
            try
            {
                await _emailService.SendEmail(emailMetaData);
            }
            catch (Exception emailEx)
            {
                Console.WriteLine($"Fout bij het verzenden van e-mail: {emailEx.Message}");
                // Hier kan je een log toevoegen of een fallback-strategie toepassen
            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> WijzigAbonnementUser(string AppUserId, int nieuwAbonnementId)
        {
            var appUser = await _context.Users.FindAsync(AppUserId) ??
                throw new ArgumentException("Geen gebruiker gevonden.");

            var huidigAbonnement = await _context.UserAbonnementen
                .Include(ua => ua.Abonnement)
                .FirstOrDefaultAsync(ua => ua.AppUserId == appUser.Id && ua.IsActive) 
                ?? throw new ArgumentException("Geen actief abonnement gevonden voor deze gebruiker.");

            var nieuwAbonnement = await _context.Abonnementen
                .FirstOrDefaultAsync(a => a.AbonnementId == nieuwAbonnementId && !a.IsStandaard && !a.IsWagenparkAbonnement) ??
                throw new ArgumentException("Geen geldig abonnement gevonden.");

            var existingQueued = await _context.UserAbonnementen
                .FirstOrDefaultAsync(ua => ua.AppUserId == appUser.Id && !ua.IsActive);
            
            if (huidigAbonnement.AbonnementId == nieuwAbonnement.AbonnementId)
            {
                throw new InvalidOperationException("Nieuw abonnement kan niet hetzelfde zijn als lopende abonnement.");
            }

            if (existingQueued != null)
            {
                throw new InvalidOperationException("Er is al een abonnement in de wachtrij voor deze gebruiker.");
            }

            if (string.IsNullOrEmpty(appUser.Email))
            {
                throw new ArgumentException("De gebruiker heeft geen geldig e-mailadres.");
            }

            var tempStartDatum = huidigAbonnement?.EndDate ?? DateTime.UtcNow;
            if (huidigAbonnement.Abonnement.IsStandaard)
            {
                huidigAbonnement.IsActive = false;

                _context.UserAbonnementen.Add(new UserAbonnement
                {
                    AppUserId = appUser.Id,
                    AbonnementId = nieuwAbonnement.AbonnementId,
                    StartDate = DateTime.UtcNow, 
                    EndDate = DateTime.UtcNow.AddMonths(1),
                    IsActive = true
                });
                _context.UserAbonnementen.Remove(huidigAbonnement);

                var emailMetaData = new EmailMetaData
                {
                    ToAddress = appUser.Email,
                    Subject = "Abonnement Wijziging Gepland",
                    Body = EmailTemplates.GetAbonnementWijzigingBevestigdBody(nieuwAbonnement.Naam)
                };
                Console.WriteLine($"sending email to {appUser.Email}");
                await _emailService.SendEmail(emailMetaData);
                Console.WriteLine("sendt email to appUser.Email");
            }
            else 
            {
                _context.UserAbonnementen.Add(new UserAbonnement
                {
                    AppUserId = appUser.Id,
                    AbonnementId = nieuwAbonnement.AbonnementId,
                    StartDate = tempStartDatum,
                    EndDate = tempStartDatum.AddMonths(1),
                    IsActive = false
                });

                var emailMetaData = new EmailMetaData
                {
                    ToAddress = appUser.Email,
                    Subject = "Abonnement Wijziging Bevestigd",
                    Body = EmailTemplates.GetAbonnementWijzigingGeplandBody(nieuwAbonnement.Naam, tempStartDatum)
                };
                await _emailService.SendEmail(emailMetaData);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task AddAbonnement(List<Abonnement> abonnements){
            await _context.Abonnementen.AddRangeAsync(abonnements);
            await _context.SaveChangesAsync();
        }
    }
}