using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.ReserveringenEnSchade;
using api.Dtos.Verhuur;
using api.Models;

namespace api.Interfaces
{
    public interface IReserveringService
    {
        /// <summary>
        /// Methode accepteerd een verhuurverzoek en gebruikt een mapper om deze om te zetten in een reservering, verander de status naar "geaccpeteerd"
        /// stuurt een email
        /// </summary>
        /// <param name="verhuurVerzoekId">id van het verzoek</param>
        /// <returns>als gelukt is het true</returns>
        Task<bool> AcceptVerhuurVerzoek(int verhuurVerzoekId); 
        /// <summary>
        /// Methode weigert een verhuurverzoek door te status te veranderen naar "geweigerd" en stuur een email met de reden
        /// </summary>
        /// <param name="weigerVerhuurVerzoekDto">id van het verzoek en de reden voor weigering</param>
        /// <returns>of het is gelukt</returns>
        Task<bool> WeigerVerhuurVerzoek(WeigerVerhuurVerzoekDto weigerVerhuurVerzoekDto);
        /// <summary>
        /// db call voor alle reserveringen die behandeld kunnen worden
        /// </summary>
        /// <returns>de reserveringen uit de db die niet afgekeurd of afgerond zijn</returns>
        Task<List<Reservering>> GetAll(); 
        /// <summary>
        /// simpele getby id db call voor reserveringen, om het object op te halen
        /// </summary>
        /// <param name="ReserveringId">id van de reservering</param>
        /// <returns>het reserverings object</returns>
        Task<Reservering> GetById(int ReserveringId); 
        /// <summary>
        /// een getbyid maar deze methode returned ook accessoires en verzekeringen
        /// </summary>
        /// <param name="ReserveringId">id van de reservering</param>
        /// <returns>het reserverings object</returns>
        Task<Reservering> GetByIdOverzicht(int ReserveringId);
        /// <summary>
        /// deze methode veranderd de status van een reservering naar uitgegeven, en veranderd de status van het voertuig naar uitgegeven
        /// zodat er overzicht gehouden kan worden op het wagenpark
        /// </summary>
        /// <param name="ReserveringId">het id van de reservering die je wil uitgeven</param>
        /// <returns>als het is gelukt is het true</returns>
        Task<bool> GeefUit (int ReserveringId); 
        /// <summary>
        /// methode voor het innemen van voertuigen en bijhouden van de status van de reservering en het voertuig, maakt ook een Factuur en verstuurd deze naar de gebruiker
        /// </summary>
        /// <param name="ReserveringId">id van de reservering die je wil innemen</param>
        /// <returns>als het is gelukt is hij true</returns>
        Task <bool> NeemIn (int ReserveringId);
        /// <summary>
        /// Maakt een SchadeFormulier object aan met de ingevulde gegevens die later kan worden verwerkt
        /// voor frontendworkers die voertuigen innemen
        /// </summary>
        /// <param name="ReserveringId">id van de reservering waarbij de schde is opgelopen</param>
        /// <param name="Schade">beschrijving van de schade</param>
        /// <param name="SchadeFoto">bytes van een foto die kan worden bijgeleverd</param>
        /// <returns>als het is gelukt is hij true</returns>
        Task<bool> MeldSchadeVanuitReservering (int ReserveringId, string Schade, IFormFile? SchadeFoto); 
        /// <summary>
        /// Maakt een SchadeFormulier object aan met de ingevulde gegevens die later kan worden verwerkt
        /// deze werkt voor backendworkers die uit voertuigen deze methode kunnen aanroepen
        /// </summary>
        /// <param name="voertuigId"></param>
        /// <param name="Schade"></param>
        /// <param name="SchadeFoto"></param>
        /// <returns></returns>
        Task<bool> MeldSchadeVanuitVoertuigId (int voertuigId, string Schade, IFormFile? SchadeFoto);
        /// <summary>
        /// Methode vraagt de db uit naar reserveringen gebaseerd op reservering.appuserid
        /// </summary>
        /// <param name="AppUserId">id van de user, geleverd uit de jwt token</param>
        /// <returns>lijst van de reserveringen van de gebruiker</returns>
        Task<List<Reservering>> GetMyReserveringen(string AppUserId); 
        /// <summary>
        /// maakt een overzicht van de belangrijke data van een reservering, waar normaal id zouden staan voor tonen aan de gebruiker
        /// wordt gebruikt bij het tonen van de reserveringsgeschiedenis aan de gebruiker
        /// </summary>
        /// <param name="reservering">het reserverings object</param>
        /// <returns></returns>
        Task<HuurGeschiedenisDto> GetHuurGeschiedenis(Reservering reservering); 
        /// <summary>
        /// methode voor het wijzigen van de reservering als dit nog minimaal een week van te voren is
        /// user kan data en voertuig weizigen
        /// </summary>
        /// <param name="wijzigReserveringDto">het id van de reserverin, de nieuw data en/of het nieuwe voertuigId</param>
        /// <returns>als het is gelukt true</returns>
        Task<bool> WijzigReservering(WijzigReserveringDto wijzigReserveringDto); 
        /// <summary>
        /// verwijderd de reservering als deze nog minimaal een week verder verwijderd is van de huidige data
        /// </summary>
        /// <param name="reserveringId">id van de reservering</param>
        /// <returns>als het is gelukt true</returns>
        Task<bool> VerwijderReservering(int reserveringId); 
        /// <summary>
        /// vraagt de db naar de kilometerprijs van een voertuig
        /// gebruikt bij berekenen van de prijs van een reservering
        /// </summary>
        /// <param name="voertuigId">id van het voertuig</param>
        /// <returns>de kilometer prijs van het voertuig</returns>
        Task <decimal> GetKilometerPrijs(int voertuigId);
    }
}