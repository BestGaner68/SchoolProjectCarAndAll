using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Betalingen;

namespace api.Interfaces
{
    public interface IBetaalService
    {
        public Task<string> BehandelCreditCardGegevens(BetaalDto betaalDto); //methode is een mock versie en heeft geen echte implementatie, zou gebruikt worden bij het verwerken van betaalgegevens
    }
}