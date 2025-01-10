using api.Dtos.Verhuur;
using api.Dtos.Voertuig;
using api.Models;

namespace api.Interfaces
{
    public interface IVoertuigService
    {
        Task<List<Voertuig>> GetAllVoertuigen(); //Methode haalt all voertuigen uit de Db, gebruikt voor het tonen van de voortuigen
        Task<List<Voertuig>> GetVoertuigenByMerk(string VoertuigMerk); //Methode filterd voertuigen uit de Db op Merk
        Task<List<Voertuig>> GetVoertuigenBySoort(string VoertuigSoort); //Methode filterd voertuigen uit de Db op Soort
        Task <VoertuigDto> GetAllVoertuigDataById (int voertuigId); //Methode stuurt data naar frontend gebaseerd op het id van de benodigde auto   
        Task<bool> CheckDatesAsync(int voertuigId, DateTime startDate, DateTime endDate); // Methode checked of voertuig gehuurd kan worden op de ingevoerde data, gebruik in het maken van een verhuurverzoek
        Task<bool> CheckStatusAsync(int voertuigId); // Methode om de huidige status van een voertuig op te vragen uit de Db
        Task<List<DateTime>> GetUnavailableDates (int voertuigId); //Methode stuurt een lijst van unavailable dates naar de frontend, gebruik in filteren van voertuigen
        Task <bool> ChangeStatusVoertuig(int voertuigId, string status); //Methode voor backoffice medewerkers om handmatig een status te kunnen aanpassen
    }
}