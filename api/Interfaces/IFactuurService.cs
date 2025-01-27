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
        public Task<Factuur> MaakFactuur(Reservering reservering, PrijsOverzichtDto prijsOverzicht, string appUserId); //methode om factuur te genereren, gebruikt bij het afronden van een reservering
        public Task<bool> StuurFactuurPerEmail(Factuur factuur); //verstuurd de gemaakte factuur via email naar de gebruiker
    }
}