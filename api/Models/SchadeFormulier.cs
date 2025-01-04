using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class SchadeFormulier
    {
        [Key]
        public int SchadeFormulierID { get; set; }
        public int VoertuigId { get; set; }
        public string Schade { get; set; } =string.Empty;
    }
}