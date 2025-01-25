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
        Task<bool> IsAvailable(int voertuigId); // Methode om de huidige status van een voertuig op te vragen uit de Db
        Task<string> GetStatus (int voertuigId); // Methode haalt de momentele status op van een voertuig
        Task<List<DateTime>> GetUnavailableDates (int voertuigId); //Methode stuurt een lijst van unavailable dates naar de frontend, gebruik in filteren van voertuigen
        Task <bool> BlokkeerVoertuig(int voertuigId, string Opmerking); //Methode voor backoffice medewerkers om handmatig een voertuig te blokkeren en eventuele opmerking bij te voeren
        Task <bool> DeBlokkeerVoertuig(int voertuigId); // Methode om de voertuigen te deblokkeren
        Task<List<SchadeFormulier>> GetAllIngediendeFormulieren(); //Methode haalt alle schadeformulieren op die nog een ingediende status hebben en dus naar gekeken moet worden
        Task<bool> BehandelSchadeMelding(int schadeformulierId, string ReparatieOpmerking); //Methode om schade te behandelen
        Task<List<Voertuig>> GetAllAvailableVoertuigen(); // Methode returned alle voertuigen die klaar voor gebruik zijn
        Task<bool> CreeerNieuwVoertuig(NieuwVoertuigDto nieuwVoertuigDto); //Methode voor aanmaken nieuwe voertuigen, De datastructuur is erg belangrijk zodat de afhankelijke voertuigstatus tabel wordt bijgewerkt bij het aanmaken en verwijderen van voertuigen
        Task <bool> WeizigVoertuig (WeizigVoertuigDto weizigVoertuigDto); // Methode voor weizigen bestaande voertuigen
        Task <bool> VerwijderVoertuig (int voertuigId); //Methode voor verwijderen voertuigen
        Task <bool> AreAnyVoertuigStatus(); // Methode wordt gebruik in de voertuigInializer class om de Db te seeden
        Task AddVoertuigen(List<Voertuig> voertuigen);// Methode wordt gebruik in de voertuigInializer class om de Db te seeden
        Task AddVoertuigStatuses(List<VoertuigData> voertuigData); // Methode wordt gebruik in de voertuigInializer class om de Db te seeden
    }
}