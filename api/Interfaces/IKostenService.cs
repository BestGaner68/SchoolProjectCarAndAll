using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IKostenService
    {
        Task<decimal> BerekenVoorschot (Reservering reservering, string AppUserId);
        Task<decimal> BerekenDaadWerkelijkPrijs (Reservering reservering, int KilometersGereden, bool isSchade);
    }
}