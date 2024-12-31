using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Interfaces
{
    public interface IDoubleDataCheckerRepo
    {
        Task<bool> UsernameTaken(string username);
        Task<bool> PhoneNumberTaken(string phoneNumber);
        Task<bool> EmailTaken(string email);
    }
}