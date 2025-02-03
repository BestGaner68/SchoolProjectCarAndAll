using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.Models
{
    /// <summary>
    /// informatie over het wagenpark, voor informatie over abonnementen zie Model.Abonnement
    /// </summary>
    public class WagenPark
    {
        [Key]
        public int WagenParkId { get; set; }
        public AppUser AppUser { get; set; }
        public string Bedrijfsnaam { get; set; } = string.Empty ;
        public string KvkNummer {get; set; } = string.Empty;
        public ICollection<WagenparkAbonnementen> WagenparkAbonnementen { get; set; } = [];
    }  
}
