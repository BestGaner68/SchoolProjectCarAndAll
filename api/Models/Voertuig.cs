using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Voertuig
    {
        public int Id { get; set; }
        public string Merk { get; set; } 
        public string Kenteken { get; set; }
        public string kleur { get; set; }  
        public int AanschafJaar { get; set; }   
    }
}