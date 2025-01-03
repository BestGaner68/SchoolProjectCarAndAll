using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IReserveringService
    {
        Task<bool> AcceptVerhuurVerzoek(int verhuurVerzoekId);
        Task<bool> WeigerVerhuurVerzoek(int verhuurVerzoekId);
        Task<List<Reservering>> GetAll();
        Task<Reservering> GetById(int ReserveringId);
    }
}