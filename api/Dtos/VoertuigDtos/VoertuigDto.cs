using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core.Pipeline;

namespace api.Dtos.VoertuigDtos
{
    public class VoertuigDto
    {
        public string Merk { get; set; } =string.Empty;
        public string type { get; set; } =string.Empty;
        public string Soort { get; set; } =string.Empty;
    }
}