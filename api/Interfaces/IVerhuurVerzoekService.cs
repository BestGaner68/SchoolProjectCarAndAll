using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IVerhuurVerzoekService
    {
        Task<List<VerhuurVerzoek>> GetAllAsync();

        Task<VerhuurVerzoek?> GetByIdAsync(int id);

        Task<VerhuurVerzoek> CreateAsync (VerhuurVerzoek verhuurVerzoek);
    }
}