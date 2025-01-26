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
        Task<bool> AcceptVerhuurVerzoek(int verhuurVerzoekId); // Methode accepteerd een verhuurverzoek en zet deze om in een reservering
        Task<bool> WeigerVerhuurVerzoek(int verhuurVerzoekId); //Methode weigert een verhuurverzoek
        Task<List<Reservering>> GetAll(); // Methode voor ophalen van alle reserveringen
        Task<Reservering> GetById(int ReserveringId); //Methode haalt reservering op uit de Db, gebaseerd op het id
        Task<bool> GeefUit (int ReserveringId); // Methode voor het uitgeven van voertuigen, houdt de status bij in de Db
        Task <bool> NeemIn (int ReserveringId); // Methode voor het innemen van voertuigen, houdt de status bij in de Db
        Task<bool> MeldSchadeVanuitReservering (int ReserveringId, string Schade, IFormFile? SchadeFoto); //Methode wordt gebruik om schade te melden als deze aanwezig is, slaat ook een bijgeleverde foto op als deze aanwezig is
        Task<bool> MeldSchadeVanuitVoertuigId (int voertuigId, string Schade, IFormFile? SchadeFoto);
        Task<List<Reservering>> GetMyReserveringen(string AppUserId); //Methode genereerd een lijst van alle afgeronde reservering van een gebruiker
        Task<HuurGeschiedenisDto> GetHuurGeschiedenis(Reservering reservering); //Methode pakt alle relevante data uit de Db
        Task<bool> WijzigReservering(WijzigReserveringDto wijzigReserveringDto);
        Task<bool> VerwijderReservering(int reserveringId);
    }
}