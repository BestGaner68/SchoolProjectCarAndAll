using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.KostenDtos;
using api.Models;

namespace api.Interfaces
{
    /// <summary>
    /// Klasse is alleen voor formatteren van data in de vorm van een factuur of het versturen hiervan, het berekenen van de kosten wordt in de Ikostenservice gedaan
    /// </summary>
    public interface IFactuurService
    {
        /// <summary>
        /// Methode maakt een factuur van de geleverde reservering, formatteerd de data uit de prijsoverzichtDto waar de kosten al netjes zijn opgeschreven
        /// </summary>
        /// <param name="reservering">reserverings object</param>
        /// <param name="prijsOverzicht">overzicht van de kosten in de vorm (totale prijs : naam , kostenberekening)</param>
        /// <param name="appUserId">id van de gebruiker waarvoor de factuur is gemaakt</param>
        /// <returns>een factuur van de data</returns>
        public Task<Factuur> MaakFactuur(Reservering reservering, PrijsOverzichtDto prijsOverzicht, string appUserId); 
        /// <summary>
        /// Verstuurd een factuur object via de email
        /// </summary>
        /// <param name="factuur">het factuur object</param>
        /// <returns>als is gelukt true</returns>
        public Task<bool> StuurFactuurPerEmail(Factuur factuur);
    }
}