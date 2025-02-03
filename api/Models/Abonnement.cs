using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    /// <summary>
    /// Isstandaard is voor het payasyougo abonnement wat makkelijke een distinctie geeft bij db calls
    /// Iswagenparkabonnement zodat het snel duidelijk is welke abonnement voor wie is, users of wagenparken
    /// 
    /// *HOE WERKEN DE ABONNEMENTEN?*
    /// beiden gebruikers en wagenparken hebben een lijst van hun abonnementen, als een abonnementen word veranderd van het standaard pay as you go abonnement is dit direct actief
    /// bij veranderingen naar andere abonnementen moet de gebruiker eerst wachten tot hun momentele abonnement is afgelopen waarna het volgende abonnement automatisch actief wordt
    /// elke gebruiker/wagenpark kan 1 actief en 1 gequeued abonnement hebben
    /// </summary>
    public class Abonnement
    {
        [Key]
        public int AbonnementId { get; set; }
        public string Naam { get; set; } = string.Empty;
        public decimal Prijs { get; set; }
        public bool IsStandaard { get; set; }
        public bool IsWagenparkAbonnement { get; set; }
        public ICollection<UserAbonnement> UserAbonnementen { get; set; } 
    }
}