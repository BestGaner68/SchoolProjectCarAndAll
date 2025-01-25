using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DataStructureClasses;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class AbonnementServiceRepo :IAbonnementService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWagenparkService _wagenparkService;
        private readonly IEmailService _emailService;
        public AbonnementServiceRepo(ApplicationDbContext context, IWagenparkService wagenparkService, IEmailService emailService)
        {
            _context=context;
            _wagenparkService = wagenparkService;
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

        public async Task<Abonnement> GetActiveAbonnement(int wagenparkId)
        {
            var wagenpark = await _context.Wagenpark
                .Include(wp => wp.WagenparkAbonnementen)
                .ThenInclude(wa => wa.Abonnement)
                .FirstOrDefaultAsync(wp => wp.WagenParkId == wagenparkId) ?? throw new Exception("Gebruiker is geen eigenaar van een wagenpark");
            var activeAbonnement = wagenpark.WagenparkAbonnementen
                .FirstOrDefault(wa => wa.IsActief);
  
            return activeAbonnement?.Abonnement;
        }

        public async Task<IEnumerable<Abonnement>> GetAllAbonnementen()
        {
            return await _context.Abonnementen.ToListAsync();
        }

        public async Task<Abonnement?> GetUserAbonnement(string appUserId)
        {
            var currentUser = await _context.Users.FindAsync(appUserId);
            if (currentUser == null) return null;
            var userWagenParkId = await _context.WagenParkUserLists
                .Where(w => w.AppUserId == appUserId)
                .Select(w => w.WagenParkId)
                .FirstOrDefaultAsync();
             
            if (userWagenParkId == 0) return null;
            var userWagenpark = await _context.Wagenpark.FindAsync(userWagenParkId);
            if (userWagenpark == null) return null;
            var abonnement = await _context.WagenparkAbonnementen
                .Where(a => a.WagenParkId == userWagenpark.WagenParkId && a.IsActief == true)
                .FirstOrDefaultAsync();

            if (abonnement == null) return null;
            return await _context.Abonnementen.FindAsync(abonnement.AbonnementId);  
        }

        public async Task<bool> WijzigAbonnementWagenpark(int wagenParkId, int nieuwAbonnementId)
        {
            var wagenPark = await _context.Wagenpark
                .Include(wp => wp.WagenparkAbonnementen)
                .FirstOrDefaultAsync(wp => wp.WagenParkId == wagenParkId) ?? 
                throw new ArgumentException("Geen wagenpark gevonden.");
            
            var nieuwAbonnement = await _context.Abonnementen
                .FirstOrDefaultAsync(a => a.AbonnementId == nieuwAbonnementId && !a.IsStandaard && a.IsWagenparkAbonnement) ?? 
                throw new ArgumentException($"Geen abonnement gevonden met id {nieuwAbonnementId}");

            var existingQueued = wagenPark.WagenparkAbonnementen
                .FirstOrDefault(wa => !wa.IsActief);

            if (existingQueued != null)
                throw new InvalidOperationException("Uw heeft al een volgend abonnement geselecteerd");

            var huidigAbonnement = wagenPark.WagenparkAbonnementen
                .FirstOrDefault(wa => wa.IsActief);
            
            var tempStartDatum = huidigAbonnement?.EindDatum ?? DateTime.UtcNow;

            if (huidigAbonnement?.Abonnement.IsStandaard == true) 
            {
                huidigAbonnement.IsActief = false;

                wagenPark.WagenparkAbonnementen.Add(new WagenparkAbonnementen
                {
                    Abonnement = nieuwAbonnement,
                    StartDatum = DateTime.UtcNow, 
                    EindDatum = DateTime.UtcNow.AddDays(90), 
                    IsActief = true
                });
                _context.Remove(huidigAbonnement);
                var emailMetaData = new EmailMetaData
                {
                    ToAddress = wagenPark.AppUser.Email,
                    Subject = "Abonnement Wijziging Bevestigd",
                    Body = EmailTemplates.GetAbonnementWijzigingBevestigdBody(nieuwAbonnement.Naam)
                };
                await _emailService.SendEmail(emailMetaData);
            }
            else 
            {
                wagenPark.WagenparkAbonnementen.Add(new WagenparkAbonnementen
                {
                    Abonnement = nieuwAbonnement,
                    StartDatum = tempStartDatum,
                    EindDatum = tempStartDatum.AddDays(90), 
                    IsActief = false
                });
                var emailMetaData = new EmailMetaData
                {
                    ToAddress = wagenPark.AppUser.Email,
                    Subject = "Abonnement Wijziging Gepland",
                    Body = EmailTemplates.GetAbonnementWijzigingGeplandBody(nieuwAbonnement.Naam, tempStartDatum)
                    
                };
                await _emailService.SendEmail(emailMetaData);
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


        
    }
}