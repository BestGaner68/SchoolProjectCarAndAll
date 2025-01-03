using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class VoertuigStatus
    {
        [Key]
        public int VoertuigId { get; set; }
        public string status { get; set; }
    }
}