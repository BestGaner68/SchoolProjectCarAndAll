using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using FluentEmail.Core.Models;

namespace api.Repositories
{
    public interface IEmailService
    {
        /// <summary>
        /// methode voor het versturen van emails, de applicatie gebruik fluentemailservices voor deze functionaliteit
        /// </summary>
        /// <param name="emailMetaData">informatie die nodig is bij het versturen van de email, reciever, subject, body</param>
        /// <returns>niets</returns>
        Task SendEmail(EmailMetaData emailMetaData);
    }
}