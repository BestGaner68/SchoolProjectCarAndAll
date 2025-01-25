using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.DataStructureClasses;

namespace api.Models
{
    public class WagenParkUserList
    {
        [Key]
        public int WagenParkUserListId { get; set; }
        public int WagenParkId { get;set;}
        public WagenPark WagenPark { get;set;}
        public string EmailAddress { get; set; } = string.Empty;
        public string? AppUserId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}