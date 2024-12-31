using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Account
{
    public class UserDataDto
    {
        public string? Username { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Voornaam { get; set; }
        public string? Achternaam { get; set; }
        public string? role { get; set; }
    }
}