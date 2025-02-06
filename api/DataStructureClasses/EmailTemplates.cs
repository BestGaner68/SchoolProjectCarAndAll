using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace api.DataStructureClasses
{
    public static class EmailTemplates
    {
        public static string GetAbonnementWijzigingBevestigdBody(string abonnementName)
        {
            return $"Uw abonnement is direct gewijzigd naar {abonnementName}.";
        }
    
        public static string GetAbonnementWijzigingGeplandBody(string abonnementName, DateTime startDate)
        {
            return $"Uw abonnement wordt gewijzigd naar {abonnementName} vanaf {startDate}.";
        }

        public static string GetAbonnementBijnaVerlopenBody(string abonnementNaam, DateTime? AbonnementEinddatum)
        {
            return $"Uw abonnement '{abonnementNaam}' verloopt op {AbonnementEinddatum:dd-MM-yyyy}. " +
                                    "U kunt het nu verlengen of wijzigen via onze portal.";
        }
        public static string GetWelkomEmailWagenParkBeheerder(string gebruikersnaam, string wachtwoord, string username)
        {
            return 
                $"Beste {gebruikersnaam},\n\n" +
                $"Welkom bij **CarAndAll**! Je account is succesvol aangemaakt. Hieronder vind je de inloggegevens voor je wagenparkbeheerderaccount:\n\n" +
                $"Gebruikersnaam: {username}\n" +
                $"Wachtwoord: {wachtwoord}\n\n" +
                "Na inloggen kun je je wachtwoord aanpassen op je profielpagina.\n\n" +
                "Je hebt nu toegang tot een **Wagenparkbeheerdersaccount** waarmee je het volgende kunt doen:\n\n" +
                "- Medewerkers toevoegen of verwijderen uit je wagenpark.\n" +
                "- Inzicht krijgen in de gebruikers die toegang hebben tot jouw wagenpark.\n" +
                "- Beheer van voertuigen en wagenpark-instellingen.\n\n" +
                "Je kunt nu inloggen via onze applicatie met de bovenstaande gegevens. Zorg ervoor dat je je wachtwoord zo snel mogelijk aanpast naar iets dat je gemakkelijk kunt onthouden.\n\n" +
                "Als je vragen hebt of problemen ondervindt, neem dan gerust contact op met onze klantenservice.\n\n" +
                "Met vriendelijke groet,\n" +
                "Het CarAndAll Team\n\n" +
                "P.S. Dit is een geautomatiseerd bericht, dus als je dit niet hebt aangevraagd, neem dan direct contact op met onze ondersteuning.";
        }

        public static string GetWagenParkBeheerWeigerEmail(string voornaam, string reden)
        {
            return $@"
                Beste {voornaam},
    
                We willen je laten weten dat je verzoek om toegang te krijgen tot het wagenparkbeheer helaas is geweigerd.
    
                Reden voor weigering:
                {reden}
    
                Als je vragen hebt, neem dan gerust contact op met onze klantenservice.
    
                Met vriendelijke groet,
                Het CarAndAll Team
            ";
        }

        public static string GetReserveringReminderBody(DateTime startDatum, string voertuigNaam)
        {
            return 
                $"Beste gebruiker,\n\n" +
                $"Dit is een herinnering dat uw reservering voor het voertuig '{voertuigNaam}' morgen ({startDatum:dd-MM-yyyy}) begint.\n\n" +
                $"Locatie: Johanna Westerdijkplein 75\n" +
                "2521 EN Den Haag\n" +
                $"Ophaaltijd: Tussen 9:00 en 15:00\n\n" +
                "Zorg ervoor dat u alles klaar heeft voor een vlotte start van uw reservering. " +
                "Volg de veiligheidsinstructies en zorg ervoor dat u alle benodigde documenten meeneemt.\n\n" +
                "Met vriendelijke groet,\nHet Team";
        }

        public static string GetUitnodigingVoorWagenparkBody(string wagenParkNaam)
        {
            return
            $"Uitnodiging voor WagenPark {wagenParkNaam}" +

            "Beste gebruiker," +

            $"U bent uitgenodigd om deel te nemen aan het WagenPark {wagenParkNaam}." +
            $"U kunt via onze website een zakelijk account om toegang te krijgen tot het wagen" +
            "Met vriendelijke groet, " + 
            "Het team van CarAndAll";
        }

        public static string RentalRequestAccepted(Reservering reservering, Voertuig voertuig)
        {
            return 
            $@"
            Beste {reservering.Fullname},

            Uw verhuurverzoek voor het voertuig van merk {voertuig.Merk} en type {voertuig.Type} is geaccepteerd.

            Startdatum: {reservering.StartDatum:dd MMM yyyy}  
            Einddatum: {reservering.EindDatum:dd MMM yyyy}  

            Er zal nog een e-mail volgen met de bijbehorende ophaallocatie en datum ter herinnering.

            Met vriendelijke groet,  
            Team CarAndAll";
        }

        public static string RentalRequestRejected(AppUser user, string reden)
        {
            return $@"
            Beste {user.Voornaam} {user.Achternaam},
    
            Het spijt ons u te moeten informeren dat uw verhuurverzoek is geweigerd.
    
            Reden: {reden}
    
            Als u vragen heeft, neem dan contact met ons op.
    
            Met vriendelijke groet,  
            Team CarAndAll";
        }


    }
}