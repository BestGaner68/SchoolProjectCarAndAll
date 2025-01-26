using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using api.Models;
using api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/test")]
    [ApiController]
    [Authorize()]
    public class TestController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public TestController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        [HttpGet]
        public IActionResult GetTest()
        {
            return Ok("Test successful");
        }
        [HttpGet("testemail")]
        public async Task TestSendEmail()
        {
            var emailMetaData = new EmailMetaData
            {
                ToAddress = "ayhan_roj@live.nl",
                Subject = "Test Email",
                Body = "This is a test email message.",
            };
            try
            {
                await _emailService.SendEmail(emailMetaData);
            }
            catch (SmtpException smtpEx)
            {

                Console.WriteLine($"SMTP Error: {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}