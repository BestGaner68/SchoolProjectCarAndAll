using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Auto : Voertuig
    {
        [Key]
        public int AutoId { get; set; }
    }
}