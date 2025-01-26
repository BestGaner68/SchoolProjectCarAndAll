using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.KostenDtos;
using api.Models;

namespace api.Interfaces
{
    public interface IFactuurService
    {
        public Task<Factuur> MaakFactuur(Reservering reservering, PrijsOverzichtDto prijsOverzicht, AppUser appUser);
    }
}