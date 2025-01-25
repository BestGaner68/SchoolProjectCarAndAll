using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using api.Models;
using FluentEmail.Core;
using FluentEmail.Core.Models;

namespace api.Repositories
{
    public class EmailRepo : IEmailService
    {
        private readonly IFluentEmail _fluentEmail;
    
        public EmailRepo(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail; 
        }

        public async Task SendEmail(EmailMetaData emailMetaData)
        {
            await _fluentEmail.To(emailMetaData.ToAddress)
            .Subject(emailMetaData.Subject)
            .Body(emailMetaData.Body)
            .SendAsync();
        }
    }
}