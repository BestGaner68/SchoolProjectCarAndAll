using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    /// <summary>
    /// geen db klasse, gebruik bij versturen emails
    /// </summary>
    public class EmailMetaData
    {
        public string ToAddress { get; set; } 
        public string Subject { get; set; } 
        public string? Body { get; set; }
    }
}