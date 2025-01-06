using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Verhuur;
using api.Models;

namespace api.Interfaces
{
    public interface IVoertuigHelper
    {   
            Task<bool> CheckDatesAsync(int voertuigId, DateTime startDate, DateTime endDate);
            Task<bool> CheckStatusAsync(int voertuigId);
            Task<List<DateTime>> GetUnavailableDates (int voertuigId);
            Task <bool> ChangeStatusVoertuig(int voertuigId, string status);
            Task <VolledigeDataDto> GetVolledigeDataDto (VerhuurVerzoek verhuurVerzoek);
            
    }
}