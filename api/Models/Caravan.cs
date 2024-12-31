using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Caravan : Voertuig
    {
        [Key]
        public int CaravanId { get; set; }
    }
}