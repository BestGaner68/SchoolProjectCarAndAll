using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IVoertuigService
    {
        Task<List<Voertuig>> GetAllVoertuigen();
        Task<List<Voertuig>> GetVoertuigenByMerk(String VoertuigMerk);
        Task<List<Voertuig>> GetVoertuigenBySoort(String VoertuigSoort);
    }
}