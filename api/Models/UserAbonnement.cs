using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    /// <summary>
    /// gebruiker heeft een lijst van userabonnementen, hoe werkt dit? zie Models.AppUser comment. niets bijzonders verder
    /// </summary>
    public class UserAbonnement
    {
        public int UserAbonnementId { get; set; }
        public string AppUserId { get; set; } = string.Empty;
        public int AbonnementId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public AppUser AppUser { get; set; } 
        public Abonnement Abonnement { get; set; } 
    }
}