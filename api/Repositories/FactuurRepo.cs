using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.KostenDtos;
using api.Interfaces;
using api.Models;

namespace api.Repositories
{
    public class FactuurRepo : IFactuurService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        public FactuurRepo(ApplicationDbContext applicationDbContext, IEmailService emailService)
        {
            _context = applicationDbContext;
            _emailService = emailService;
        }
        public async Task<Factuur> MaakFactuur(Reservering reservering, PrijsOverzichtDto prijsOverzicht,string appUserId)
        {
            var user = await _context.Users.FindAsync(appUserId) ?? throw new Exception("No user found");
            var factuur = new Factuur
            {
                Factuurnummer = "INV" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                KlantNaam = $"{user.Voornaam} {user.Achternaam}",
                KlantEmail = user.Email,
                Bedrag = prijsOverzicht.TotalePrijs,
                Datum = DateTime.Now,
                PrijsDetails = prijsOverzicht.PrijsDetails.Select(detail => new PrijsOnderdeelDto
                {
                    Beschrijving = detail.Beschrijving,
                    Amount = detail.Amount
                }).ToList()
            };
            return factuur;
        }

        public async Task<bool> StuurFactuurPerEmail(Factuur factuur)
        {
            string emailBody = GenereerFactuurBody(factuur);
        
            var emailMetadata = new EmailMetaData
            {
                ToAddress = factuur.KlantEmail,
                Subject = "Uw Factuur voor Reservering",
                Body = emailBody
            };
        Console.WriteLine($"{emailBody}");
            await _emailService.SendEmail(emailMetadata);
            return true;
        }

        private string GenereerFactuurBody(Factuur factuur)
        {
            StringBuilder body = new StringBuilder();

            body.AppendLine($"Factuurnummer: {factuur.Factuurnummer}");
            body.AppendLine($"Datum: {factuur.Datum.ToString("dd-MM-yyyy")}");
            body.AppendLine($"Klant: {factuur.KlantNaam}");
            body.AppendLine($"Totaalbedrag: €{factuur.Bedrag.ToString("0.00")}");

            body.AppendLine("\nPrijsdetails:");
            foreach (var detail in factuur.PrijsDetails)
            {
                body.AppendLine($"{detail.Beschrijving}: €{detail.Amount.ToString("0.00")}");
            }

            body.AppendLine("\nHartelijk dank voor uw reservering!");

            return body.ToString();
        }
    }   
}  