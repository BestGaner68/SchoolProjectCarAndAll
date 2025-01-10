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
        Task<bool> AcceptVerhuurVerzoek(int verhuurVerzoekId);
        Task<bool> WeigerVerhuurVerzoek(int verhuurVerzoekId);
        Task<List<Reservering>> GetAll();
        Task<Reservering> GetById(int ReserveringId);
        Task<bool> GeefUit (int ReserveringId);
        Task <bool> NeemIn (int ReserveringId);
        Task<bool> MeldSchade (int ReserveringId, string Schade);
        Task<List<Reservering>> GetMyReserveringen(string AppUserId); //Methode genereerd een lijst van alle afgeronde reservering van een gebruiker
        Task<HuurGeschiedenisDto> GetHuurGeschiedenis(Reservering reservering); //Methode pakt alle relevante data uit de Db
    }
}