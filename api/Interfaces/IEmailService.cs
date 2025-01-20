using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Repositories
{
    public interface IEmailService
    {
        Task SendEmail(string to, string subject, string body);
        Task SendWagenParkBeheerderWelcomeEmail(string to, string username, string password);
        Task SendWagenParkBeheerWeigerEmail (string to, string? Reden, string Voornaam);
    }
}