using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Betalingen;

namespace api.Interfaces
{
    public interface IBetaalService
    {
        public Task<string> BehandelCreditCardGegevens(BetaalDto betaalDto);
    }
}