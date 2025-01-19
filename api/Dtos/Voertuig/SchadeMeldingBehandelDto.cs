using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Voertuig
{
    public class SchadeMeldingBehandelDto
    {
        [Required]
        public int SchadeFormulierId;
        public string ReparatieOpmerking = string.Empty;
    }
}