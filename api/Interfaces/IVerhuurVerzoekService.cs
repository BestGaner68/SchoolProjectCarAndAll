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
        Task<List<VerhuurVerzoek>> GetAllAsync(); //Methode om alle verhuurverzoeken uit de Db te halen
        Task<VerhuurVerzoek?> GetByIdAsync(int id); //Methode die een verhuurverzoek op id ophaalt uit de Db
        Task<VerhuurVerzoek> CreateAsync (VerhuurVerzoek verhuurVerzoek); //Methode om verhuurverzoeken te schrijven naar de Db
        Task<List<VolledigeDataDto>> GetPendingAsync(); //Methode haalt alle verhuurverzoeken op die momenteel op Pending status staan in de Db
        Task<List<VerhuurVerzoek>> GetMyVerhuurVerzoeken(string AppUserId); //Methode om de verhuurverzoeken van een gebruiker uit de Db te vragen
        Task <bool>DeclineMyVerzoek(int verhuurVerzoekId, string AppUserId); //Methode om de status van het verhuurverzoek aan te passen in de database, checked of de UserIds Overeenkomen
        Task<List<Reservering>> ViewHuurGeschiedenis (string AppUserId); //Methode om huurgeschiedenis van een gebruiker uit de Db
        Task <List<Verzekering>> GetAllVerzekeringen ();
        Task <List<Accessoires>> GetAllAccessoires ();
        Task <List<Accessoires>> FromIdToInstanceAccessoires(List<int?> AccessoiresList);
        Task <Verzekering> FromIdToInstanceVerzekering (int verzekeringId);
        Task<VerhuurVerzoek> GetByIdOverzichtVerhuurverzoek(int VerhuurverzoekId);
    
    }
}