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
        Task SendEmail(EmailMetaData emailMetaData); //simple methode gebruikt om emails te versturen
    }
}