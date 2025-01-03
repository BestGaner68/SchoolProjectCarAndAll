using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Interfaces
{
    public interface IVoertuigHelper
    {   
            Task<bool> CheckDatesAsync(int voertuigId, DateTime startDate, DateTime endDate);
            Task<bool> CheckStatusAsync(int voertuigId);
            Task<List<DateTime>> GetUnavailableDates (int voertuigId);
    }
}