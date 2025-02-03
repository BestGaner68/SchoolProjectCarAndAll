using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace api.Service
{
    /// <summary>
    ///  initalisatie van de emailservice, hier worden de gegevens uit de appsetting gelezen
    /// </summary>
    public static class FluentEmailExtensions
    {
        public static void AddFluentEmail(this IServiceCollection services, IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection("EmailSettings");
            var smtpSettings = emailSettings.GetSection("SmtpSetting");

            services.AddFluentEmail(emailSettings["DefaultFromEmail"])
                .AddSmtpSender(new SmtpClient(smtpSettings["Host"])
                {
                    Port = smtpSettings.GetValue<int>("Port"),
                    Credentials = new NetworkCredential(
                        smtpSettings["Username"],
                        smtpSettings["Password"]
                    ),
                    EnableSsl = smtpSettings.GetValue<bool>("EnableSsl")
                });
        }
    }
}