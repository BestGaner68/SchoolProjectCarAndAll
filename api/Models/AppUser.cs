using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    /// <summary>
    /// IdentityUser is door identityframework geleverde klasse die gebruikt is bij het aanmaken van accounts 
    /// verder heb ik voor en achternaam toegevoegt. voor informatie over de lijst van userabonnementen zie Models.Abonnementen
    /// </summary>
    public class AppUser : IdentityUser
    {
        public string? Voornaam { get; set; }
        public string? Achternaam { get; set; }
        public ICollection<UserAbonnement> UserAbonnementen { get; set; }
    }
}