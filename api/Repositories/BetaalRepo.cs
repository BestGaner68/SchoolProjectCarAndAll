using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Betalingen;
using api.Interfaces;

namespace api.Repositories
{
    public class BetaalRepo : IBetaalService
    {
        public Task<string> BehandelCreditCardGegevens(BetaalDto betaalDto)
        {
            string resultaat = $"Creditcard verwerkt voor {betaalDto.CardHolder}";
            return Task.FromResult(resultaat);
        }
    }
}