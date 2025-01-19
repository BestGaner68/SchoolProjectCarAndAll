using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;

namespace api.Repositories
{
    public class KostenRepo : IKostenService
    {
        public Task<decimal> BerekenDaadWerkelijkPrijs(Reservering reservering, int KilometersGereden, bool isSchade)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> BerekenVoorschot(Reservering reservering, string AppUserId)
        {
            throw new NotImplementedException();
        }
    }
}