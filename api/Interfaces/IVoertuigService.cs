using api.Dtos.ReserveringenEnSchade;
using api.Dtos.Verhuur;
using api.Dtos.VoertuigDtos;
using api.Models;

namespace api.Interfaces
{
    public interface IVoertuigService
    {
        /// <summary>
        /// methode haalt alle voertuigen uit de database, gebruikt voor het tonen van de voertuigen in de frontend
        /// </summary>
        /// <returns>een lijst van alle voertuigen</returns>
        Task<List<Voertuig>> GetAllVoertuigen();

        /// <summary>
        /// Methode filtert voertuigen uit de database op merk
        /// </summary>
        /// <param name="VoertuigMerk">Het merk van het voertuig</param>
        /// <returns>Een lijst van voertuigen met het opgegeven merk</returns>
        Task<List<Voertuig>> GetVoertuigenByMerk(string VoertuigMerk);

        /// <summary>
        /// Methode filtert voertuigen uit de database op beschikbaarheid binnen een opgegeven datumperiode
        /// </summary>
        /// <param name="datumdto">Een DTO met de gewenste datumbereik</param>
        /// <returns>Een lijst van beschikbare voertuigen binnen het datumbereik</returns>
        Task<List<Voertuig>> GetVoertuigenByDate(DatumDto datumdto);

        /// <summary>
        /// Methode filtert voertuigen uit de database op voertuigsoort
        /// </summary>
        /// <param name="VoertuigSoort">De soort van het voertuig</param>
        /// <returns>Een lijst van voertuigen met het opgegeven type</returns>
        Task<List<Voertuig>> GetVoertuigenBySoort(string VoertuigSoort);

        /// <summary>
        /// Methode stuurt een VoertuigDto met het merk soort en type van het voertuig naar de frontend gebaseerd op het voertuig ID
        /// </summary>
        /// <param name="voertuigId">Het ID van het voertuig</param>
        /// <returns>Een DTO met basisinformatie over het voertuig</returns>
        Task<VoertuigDto> GetSimpleVoertuigDataById(int voertuigId);

        /// <summary>
        /// Methode wordt gebruikt voor frontendmedewerkers om voertuiggegevens en bijbehorende schadeformulieren op te halen uit de db
        /// </summary>
        /// <param name="voertuigId">Het ID van het voertuig</param>
        /// <returns>Een DTO met alle gegevens van het voertuig</returns>
        Task<VolledigeVoertuigDataDto> GetAllDataVoertuig(int voertuigId);

        /// <summary>
        /// Methode controleert of een voertuig beschikbaar is voor verhuur binnen een specifieke periode
        /// </summary>
        /// <param name="voertuigId">Het ID van het voertuig</param>
        /// <param name="startDate">De begindatum van de huurperiode</param>
        /// <param name="endDate">De einddatum van de huurperiode</param>
        /// <returns>True als het voertuig beschikbaar is, anders false</returns>
        Task<bool> CheckDatesAsync(int voertuigId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Methode om de huidige status van een voertuig uit de database op te vragen, kijkt of het voertuig "Klaarvoorgebruik" is
        /// </summary>
        /// <param name="voertuigId">Het ID van het voertuig</param>
        /// <returns>True als het voertuig beschikbaar is, anders false</returns>
        Task<bool> IsAvailable(int voertuigId);

        /// <summary>
        /// Methode haalt de actuele status van een voertuig op
        /// </summary>
        /// <param name="voertuigId">Het ID van het voertuig</param>
        /// <returns>De status van het voertuig als string</returns>
        Task<string> GetStatus(int voertuigId);

        /// <summary>
        /// Methode stuurt een lijst van onbeschikbare data naar de frontend gebaseerd op het id van het voertuig, zodat het niet verhuur kan worden in die periode
        /// </summary>
        /// <param name="voertuigId">Het ID van het voertuig</param>
        /// <returns>Een lijst van data waarop het voertuig niet beschikbaar is</returns>
        Task<List<DateTime>> GetUnavailableDates(int voertuigId);

        /// <summary>
        /// Methode voor backofficemedewerkers om een voertuig handmatig te blokkeren en een opmerking toe te voegen
        /// </summary>
        /// <param name="voertuigId">Het ID van het voertuig</param>
        /// <param name="Opmerking">De reden voor de blokkade</param>
        /// <returns>True als het blokkeren is gelukt</returns>
        Task<bool> BlokkeerVoertuig(int voertuigId, string Opmerking);

        /// <summary>
        /// Methode om voertuigen te deblokkeren zodat ze weer gebruikt kunnen worden
        /// </summary>
        /// <param name="voertuigId">Het ID van het voertuig</param>
        /// <returns>True als het deblokkeren is gelukt</returns>
        Task<bool> DeBlokkeerVoertuig(int voertuigId);

        /// <summary>
        /// Methode haalt alle ingediende schadeformulieren op die nog beoordeeld moeten worden, gebaseerd op de status
        /// </summary>
        /// <returns>Een lijst van schadeformulieren</returns>
        Task<List<SchadeFormulier>> GetAllIngediendeFormulieren();

        /// <summary>
        /// Methode om schadeformulieren te verwerken en eventueel een reparatieopmerking toe te voegen, hierbij moet de schade op het voertuig
        /// daadwerkelijk verwerkt worden
        /// </summary>
        /// <param name="schadeformulierId">Het ID van het schadeformulier.</param>
        /// <param name="ReparatieOpmerking">Een opmerking over de reparatie.</param>
        /// <returns>True als de schadeverwerking is gelukt.</returns>
        Task<bool> BehandelSchadeMelding(int schadeformulierId, string ReparatieOpmerking);

        /// <summary>
        /// Methode retourneert alle voertuigen met status "klaarvoorgebruik".
        /// </summary>
        /// <returns>Een lijst van beschikbare voertuigen.</returns>
        Task<List<Voertuig>> GetAllAvailableVoertuigen();

        /// <summary>
        /// Methode voor het aanmaken van een nieuw voertuig. Zorgt ervoor dat afhankelijke voertuigstatus tabellen correct worden bijgewerkt.
        /// </summary>
        /// <param name="nieuwVoertuigDto">De gegevens van het nieuwe voertuig zoals kenteken, voertuignaam, type, merk, etc.</param>
        /// <returns>True als het voertuig succesvol is aangemaakt.</returns>
        Task<bool> CreeerNieuwVoertuig(NieuwVoertuigDto nieuwVoertuigDto);
        /// <summary>
        /// Methode voor het weizigen van de data van een voertuig. merk, kenteken etc. zijn nullable bij geen weiziging
        /// </summary>
        /// <param name="weizigVoertuigDto">de gegevens die moeten worden geweizigd</param>
        /// <returns>true als het is gelukt</returns>
        Task <bool> WeizigVoertuig (WeizigVoertuigDto weizigVoertuigDto);
        /// <summary>
        /// verwijderd een voertuig uit de db en de bijbehorende voertuigdata tabel
        /// </summary>
        /// <param name="voertuigId">het id van het voertuig</param>
        /// <returns>true als het is gelukt</returns>
        Task <bool> VerwijderVoertuig (int voertuigId);
        /// <summary>
        /// check of er al instanties van voertuigdata zijn in de db, gebruik bij seeden db
        /// </summary>
        /// <returns>true als de db al data heeft</returns>
        Task <bool> AreAnyVoertuigData(); 
        /// <summary>
        /// Methode voor het invoeren van voertuigen in de database
        /// </summary>
        /// <param name="voertuigen">lijst van alle voertuigen die geseed moeten worden</param>
        /// <returns>niets</returns>
        Task AddVoertuigen(List<Voertuig> voertuigen);// Methode wordt gebruik in de voertuigInializer class om de Db te seeden
    }
}