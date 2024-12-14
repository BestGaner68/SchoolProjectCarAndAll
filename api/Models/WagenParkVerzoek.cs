using System.ComponentModel.DataAnnotations;
using api.Models;

namespace api.Models
{
    public class WagenParkVerzoek
    {
        [Key]
        public int wagenparkverzoekId {get; set;}
        public AppUser? appUser {get;set;}
        public string AppUserId {get;set;} = string.Empty;
        public WagenPark? wagenPark{get;set;}
        public int WagenparkId {get;set;}
    }
}
