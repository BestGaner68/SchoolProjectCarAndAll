using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.KostenDtos;
using api.Models;

namespace api.Interfaces
{
    public interface IKostenService
    {
        Task<PrijsOverzichtDto> BerekenTotalePrijs(int reserveringId, bool isSchade, decimal kilometersGereden);
        Task<PrijsOverzichtDto> BerekenVerwachtePrijsUitVerhuurVerzoek(int VerhuurverzoekId);
    }
}