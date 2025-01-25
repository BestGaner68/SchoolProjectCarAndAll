using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class AppUser : IdentityUser
    {
        public string? Voornaam { get; set; }
        public string? Achternaam { get; set; }
        public ICollection<UserAbonnement> UserAbonnementen { get; set; }
    }
}