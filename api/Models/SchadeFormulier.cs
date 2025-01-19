using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using api.DataStructureClasses;

namespace api.Models
{
    public class SchadeFormulier
    {
        [Key]
        public int SchadeFormulierID { get; set; }
        public int VoertuigId { get; set; }
        public string Schade { get; set; } =string.Empty;
        public String Status { get; set; } = SchadeStatus.Ingediend;
        public DateTime SchadeDatum { get; set; }
        public string ReparatieOpmerking { get; set; } = string.Empty;
        public byte[]? SchadeFoto {get; set;}
    }
}