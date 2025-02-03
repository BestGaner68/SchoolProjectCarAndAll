using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DataStructureClasses;
using api.Models;
using api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace api.Service
{
    /// <summary>
    /// deze klasse runt altijd tijdens de applicatie en checked elke 12 uur of er nog iets moet gebeuren zoals: emails versturen, abonnementen omzetten
    /// of of een reservering nog mag worden aangepast en zo niet word de status aangepast.
    /// </summary>
    public class NotificationHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<NotificationHostedService> _logger;

        public NotificationHostedService(IServiceProvider serviceProvider, ILogger<NotificationHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        /// <summary>
        /// Main methode die om de 12 uur aangeroepen wordt. maakt een call naar alle andere methodes hieronder
        /// </summary>
        /// <param name="stoppingToken">Geeft aan wanneer de service moet stoppen (eigenlijk nooit)</param>
        /// <returns>niets</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("NotificationHostedService is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                    await SendAbonnementExpiryNotifications(context, emailService, stoppingToken);
                    await ProcessAbonnementSwitches(context, emailService, stoppingToken);
                    await SendReserveringStartNotifications(context, emailService, stoppingToken);
                    await UpdateReservingStatuses(context, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred in NotificationHostedService.");
                }
                await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
            }
        }

        /// <summary>
        /// stuurd een notificatie naar de gebruiker als het abonnement bijna afloopt (nog 1 maand over)
        /// de gebruiker kan daarna zijn abonnement verlengen
        /// </summary>
        /// <param name="context">voor db calls</param>
        /// <param name="_emailService">voor emails versturen</param>
        /// <param name="stoppingToken">voor aangeven wanneer het moet stoppen</param>
        /// <returns>niets</returns>

        private async Task SendAbonnementExpiryNotifications(
            ApplicationDbContext context, 
            IEmailService _emailService, 
            CancellationToken stoppingToken)
        {
            var now = DateTime.UtcNow;
            var reminderThreshold = now.AddMonths(1);

            var expiringAbonnementen = await context.WagenparkAbonnementen
                .Include(wa => wa.WagenPark)
                .ThenInclude(wp => wp.AppUser)
                .Where(wa => wa.IsActief && wa.EindDatum <= reminderThreshold && wa.EindDatum > now)
                .ToListAsync(stoppingToken);

            foreach (var abonnement in expiringAbonnementen)
            {
                var userEmail = abonnement.WagenPark.AppUser.Email;

                if (!string.IsNullOrEmpty(userEmail))
                {
                    var emailMetaData = new EmailMetaData
                    {
                        ToAddress = userEmail,
                        Subject = "Abonnement bijna verlopen",
                        Body = EmailTemplates.GetAbonnementBijnaVerlopenBody(abonnement.Abonnement.Naam, abonnement.EindDatum)
                    };
                    await _emailService.SendEmail(emailMetaData);

                    _logger.LogInformation($"Expiry notification sent to {userEmail} for abonnement {abonnement.Abonnement.Naam}.");
                }
            }
        }

        /// <summary>
        /// veranderd de abonnementen die omgezet moeten worden. dus verwijderd actieve abonnement en zet het "gequeuede" abonnement op actief
        /// </summary>
        /// <param name="context"></param> **zie boven
        /// <param name="emailService"></param>
        /// <param name="stoppingToken"></param>
        /// <returns>niets</returns>
        private async Task ProcessAbonnementSwitches(
            ApplicationDbContext context,
            IEmailService emailService,  
            CancellationToken stoppingToken)
        {
            var now = DateTime.UtcNow;

            var abonnementenToSwitch = await context.WagenparkAbonnementen
                .Include(wa => wa.WagenPark)
                .ThenInclude(wp => wp.AppUser)
                .Where(wa => wa.IsActief && wa.EindDatum <= now)
                .ToListAsync(stoppingToken);

            foreach (var abonnement in abonnementenToSwitch)
            {
                abonnement.IsActief = false;
                var queuedAbonnement = await context.WagenparkAbonnementen
                    .Where(wa => wa.WagenParkId == abonnement.WagenParkId && !wa.IsActief && wa.StartDatum == abonnement.EindDatum)
                    .OrderBy(wa => wa.StartDatum)
                    .FirstOrDefaultAsync(stoppingToken);

                if (queuedAbonnement != null)
                {
                    queuedAbonnement.IsActief = true;
                    context.WagenparkAbonnementen.Remove(abonnement);
                    _logger.LogInformation($"Switched to queued abonnement '{queuedAbonnement.Abonnement.Naam}' for WagenPark {queuedAbonnement.WagenParkId}.");
                }
                else
                {
                    var payAsYouGo = await context.Abonnementen
                        .FirstOrDefaultAsync(a => a.IsStandaard, stoppingToken);

                    if (payAsYouGo != null)
                    {
                        context.WagenparkAbonnementen.Add(new WagenparkAbonnementen
                        {
                            WagenParkId = abonnement.WagenParkId,
                            AbonnementId = payAsYouGo.AbonnementId,
                            StartDatum = now,
                            EindDatum = DateTime.MaxValue,
                            IsActief = true
                        });
                    
                        _logger.LogInformation($"Reverted to default abonnement 'Pay As You Go' for WagenPark {abonnement.WagenParkId}.");
                    }
                }
                context.WagenparkAbonnementen.Remove(abonnement);
            }
            
            await context.SaveChangesAsync(stoppingToken);
        }
        /// <summary>
        /// Stuurd een email naar gebruikers waarvan hun reservering morgen start zodat ze alle belangrijke info hebben
        /// </summary>
        /// <param name="_context"></param> **zie boven
        /// <param name="_emailService"></param>
        /// <param name="stoppingToken"></param>
        /// <returns>niets</returns>
        private async Task SendReserveringStartNotifications(
        ApplicationDbContext _context,
        IEmailService _emailService,
        CancellationToken stoppingToken)
        {
            var now = DateTime.UtcNow.Date; 
            var reminderThreshold = now.AddDays(1); 
            var upcomingReserveringen = await _context.Reservering
                .Where(r => r.StartDatum.Date == reminderThreshold && r.Status == ReserveringStatussen.ReadyForPickUp)
                .ToListAsync(stoppingToken);

            foreach (var reservering in upcomingReserveringen)
            {
                var User = await _context.Users.FindAsync(reservering.AppUserId);
                var voertuig = await _context.Voertuig.FindAsync(reservering.VoertuigId);

                if (User != null && !string.IsNullOrEmpty(User.Email))
                {
                    var emailMetaData = new EmailMetaData
                    {
                        ToAddress = User.Email,
                        Subject = "Herinnering: Uw reservering start morgen",
                        Body = EmailTemplates.GetReserveringReminderBody(reservering.StartDatum, voertuig.Type)
                    };
                    await _emailService.SendEmail(emailMetaData);
                }
            }
        }

        /// <summary>
        /// voor het updaten van de statussen van reserveringen van "magwordengeweizigd" naar "readyforpickup" zodat het niet meer kan worden geweizigd
        /// </summary>
        /// <param name="context"></param>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        private async Task UpdateReservingStatuses(ApplicationDbContext context, CancellationToken stoppingToken)
        {
            var now = DateTime.UtcNow;
            var reserveringen = await context.Reservering
                .Where(r => r.Status == "MagWordenGewijzigd" && r.StartDatum > now)
                .ToListAsync(stoppingToken);

            foreach (var reservering in reserveringen)
            {
                if ((reservering.StartDatum - now).TotalDays <= 7)
                {
                    reservering.Status = "ReadyForPick";
                    await context.SaveChangesAsync(stoppingToken);
                    _logger.LogInformation($"Reservation {reservering.ReserveringId} status changed to ReadyForPick.");
                }
            }
        }

    }
}