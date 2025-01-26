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
    public class NotificationHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<NotificationHostedService> _logger;

        public NotificationHostedService(IServiceProvider serviceProvider, ILogger<NotificationHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

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
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

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
        
        private async Task SendReserveringStartNotifications(
        ApplicationDbContext _context,
        IEmailService _emailService,
        CancellationToken stoppingToken)
        {
            var now = DateTime.UtcNow.Date; 
            var reminderThreshold = now.AddDays(1); 
            var upcomingReserveringen = await _context.Reservering
                .Include(r => r.AppUserId) 
                .Where(r => r.StartDatum.Date == reminderThreshold && r.Status == ReserveringStatussen.Geaccepteerd)
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