using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Migrations;

namespace api.Models
{
    public class WagenparkLinkedUser
    {
    public int WagenparkId { get; set; }
    public WagenPark Wagenpark { get; set; }

    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    }
}