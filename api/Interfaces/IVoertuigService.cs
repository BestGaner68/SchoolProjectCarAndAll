using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IVoertuigService
    {
        Task<List<Auto>> GetAllAutos();
        Task<List<Camper>> GetAllCampers();
        Task<List<Caravan>> GetAllCaravans();
    }
}