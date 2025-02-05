using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Service
{
    /// <summary>
    /// nodig voor het versturen van emails
    /// </summary>
    public class EmailSettings
    {
        public string SmtpServer { get; set; } =string.Empty;
        public int SmtpPort { get; set; }
        public string EmailUsername { get; set; } =string.Empty;
        public string EmailPassword { get; set; } =string.Empty;
        public bool EnableSsl { get; set; }
    }
}