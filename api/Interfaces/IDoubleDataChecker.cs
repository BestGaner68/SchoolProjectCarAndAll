using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Interfaces
{
    public interface IDoubleDataCheckerRepo
    {
        Task<bool> UsernameTaken(string username); //deze methodes checken of de data al gebruikt is bij het weizigen van de gegevens van een gebruiker
        Task<bool> PhoneNumberTaken(string phoneNumber);
        Task<bool> EmailTaken(string email);
    }
}