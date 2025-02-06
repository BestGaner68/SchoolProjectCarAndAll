using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Verhuur;
using api.Models;

namespace api.Interfaces
{
    public interface IVerhuurVerzoekService
    {
        /// <summary>
        /// simpele getbyId voor verhuurverzoek object ophalen uit de db
        /// </summary>
        /// <param name="id">id van het verhuurverzoek</param>
        /// <returns>het verhuurverzoek </returns>
        Task<VerhuurVerzoek?> GetByIdAsync(int id); 
        /// <summary>
        /// methode slaat een verhuurverzoek op in de database, uit een verhuurverzoek object.
        /// dit wordt gemapt door een bijbehorende mapper
        /// </summary>
        /// <param name="verhuurVerzoek">het verhuurverzoek object</param>
        /// <returns>het verhuurverzoek object</returns>
        Task<VerhuurVerzoek> CreateAsync (VerhuurVerzoek verhuurVerzoek); 
        /// <summary>
        /// Vraagt alle verhuurverzoek op uit de db met status : pending om te verwerken
        /// </summary>
        /// <returns>alle verhuurverzoeken met status pending</returns>
        Task<List<VerhuurVerzoek>> GetPendingAsync(); 
        /// <summary>
        /// zoekt extra bijbehorende data op bij een verhuurverzoek gebaseerd op sommige id zoals voertuigId, of appuser naam
        /// </summary>
        /// <param name="verhuurVerzoek">het verhuurverzoek object</param>
        /// <returns>een volledigdata dto met de belangrijke data opgeschreven zoals voertuignaam of volldige naam van de gebruiker</returns>
        Task <VolledigeDataDto> GetVolledigeDataDto (VerhuurVerzoek verhuurVerzoek); 
        /// <summary>
        /// returned de verhuurverzoekn die een gebruiker heeft gemaakt gebaseerd op de userId
        /// </summary>
        /// <param name="AppUserId">het Id van de user uit de jwt token</param>
        /// <returns>alle verhuurverzoeken van de gebruiker</returns>
        Task<List<VerhuurVerzoek>> GetMyVerhuurVerzoeken(string AppUserId); 
        /// <summary>
        /// Gebruik als een user zijn verhuurverzoek wil cancelen en veranderd de status naar verwijderd door gebruiker zodat hij niet meer verwerkt kan worden
        /// </summary>
        /// <param name="verhuurVerzoekId">het id van het verhuurverzoek</param>
        /// <param name="AppUserId">het id van de gebruiker om te checken of hij de eigenaar is</param>
        /// <returns>als het is gelukt true</returns>
        Task <bool>DeclineMyVerzoek(int verhuurVerzoekId, string AppUserId);
        /// <summary>
        /// selecteerd alle verzekeringen uit de db
        /// </summary>
        /// <returns> alle verzekeringen uit de db voor tonen in frontend</returns>
        Task <List<Verzekering>> GetAllVerzekeringen ();
        /// <summary>
        /// selecteerd alle Accessoiresen uit de db
        /// </summary>
        /// <returns> alle Accessoiresen uit de db voor tonen in frontend</returns>
        Task <List<Accessoires>> GetAllAccessoires ();
        /// <summary>
        /// map methode die accessoires objecten ophaalt uit de db gebaseerd op id, gebruik bij aanmaken verhuurverzoeken
        /// </summary>
        /// <param name="AccessoiresList">lijst van de ids van de accessoires</param>
        /// <returns>de accessoires objecten</returns>
        Task <List<Accessoires>> FromIdToInstanceAccessoires(List<int?> AccessoiresList);
        /// <summary>
        /// haalt verzekering object op uit de db gebaseerd op id
        /// </summary>
        /// <param name="verzekeringId">het Id van de verzekering</param>
        /// <returns>de verzekering object</returns>
        Task <Verzekering> FromIdToInstanceVerzekering (int verzekeringId);
    }
}