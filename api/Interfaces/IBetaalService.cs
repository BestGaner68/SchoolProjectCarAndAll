using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Betalingen;

namespace api.Interfaces
{
    public interface IBetaalService
    {
        /// <summary>
        /// mock methode voor behandelen betaalgegevens
        /// </summary>
        /// <param name="betaalDto">betaal informatie, creditcardnummer etc.</param>
        /// <returns>mock string, dat het is gelukt</returns>
        public Task<string> BehandelCreditCardGegevens(BetaalDto betaalDto);
    }
}