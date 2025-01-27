using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Betalingen
{
    public class BetaalDto
    {
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string CardHolder { get; set; }
         [Required]
        public string ExpirationDate { get; set; }
         [Required]
        public string CVV { get; set; }
    }
}