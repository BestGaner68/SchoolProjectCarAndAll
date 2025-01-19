using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class WagenparkLinkedUser
    {
    [Key]
    public int WagenparkLinkedUserId {get; set;}
    public int WagenparkId { get; set; }
    public WagenPark? Wagenpark { get; set; }
    public string? AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    }
}