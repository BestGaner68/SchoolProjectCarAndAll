using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace api.Repositories
{
    public class EmailRepo : IEmailService
    {
        private readonly SmtpClient _smtpClient;
    
        public EmailRepo(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public async Task SendEmail(string to, string subject, string body)
        {
            var message = new MailMessage("no-reply@jouwbedrijf.nl", to, subject, body);
            await _smtpClient.SendMailAsync(message);
        }

        public async Task SendWagenParkBeheerderWelcomeEmail(string to, string username, string password)
        {
            var subject = "Welkom bij CarAndAll - Je Account Is Actief!";

            var body = $"Beste {username},\n\n" +
                       $"Welkom bij **CarAndAll**! Je account is succesvol aangemaakt. Hieronder vind je de inloggegevens voor je wagenparkbeheerderaccount:\n\n" +
                       $"**Gebruikersnaam**: {username}\n" +
                       $"**Wachtwoord**: {password}\n\n" +
                       "Na inloggen kun je je wachtwoord aanpassen op je profielpagina.\n\n" +
                       "Je hebt nu toegang tot een **Wagenparkbeheerdersaccount** waarmee je het volgende kunt doen:\n\n" +
                       "- **Medewerkers toevoegen of verwijderen** uit je wagenpark.\n" +
                       "- **Inzicht krijgen in de gebruikers** die toegang hebben tot jouw wagenpark.\n" +
                       "- **Beheer van voertuigen en wagenpark-instellingen**.\n\n" +
                       "Je kunt nu inloggen via onze applicatie met de bovenstaande gegevens. Zorg ervoor dat je je wachtwoord zo snel mogelijk aanpast naar iets dat je gemakkelijk kunt onthouden.\n\n" +
                       "Als je vragen hebt of problemen ondervindt, neem dan gerust contact op met onze klantenservice.\n\n" +
                       "Met vriendelijke groet,\n" +
                       "Het CarAndAll Team\n\n" +
                       "P.S. Dit is een geautomatiseerd bericht, dus als je dit niet hebt aangevraagd, neem dan direct contact op met onze ondersteuning.";

            await SendEmail(to, subject, body);
        }

        public async Task SendWagenParkBeheerWeigerEmail(string to, string Voornaam, string? Reden)
        {
            var subject = "Wagenparkbeheer Verzoek Geweigerd";
            var body = $@"
                Beste {Voornaam},

                We willen je laten weten dat je verzoek om toegang te krijgen tot het wagenparkbeheer helaas is geweigerd.

                Reden voor weigering:
                {Reden}

                Als je vragen hebt, neem dan gerust contact op met onze klantenservice.

                Met vriendelijke groet,
                CarAndAll Team";

            if (!(Reden == null))
            {
                body = $@"
                Beste {Voornaam},

                We willen je laten weten dat je verzoek om toegang te krijgen tot het wagenparkbeheer helaas is geweigerd.

                Als je vragen hebt, neem dan gerust contact op met onze klantenservice.

                Met vriendelijke groet,
                CarAndAll Team";
            }

            await SendEmail(to, subject, body);
        }
    }
}