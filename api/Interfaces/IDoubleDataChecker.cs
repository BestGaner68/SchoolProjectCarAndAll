using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Interfaces
{
    /// <summary>
    /// klasse is specifiek voor het checken of bepaalde data al gebruikt is in de applicatie
    /// </summary>
    public interface IDoubleDataCheckerRepo
    {
        /// <summary>
        /// checkt of de username al bestaat in de db
        /// </summary>
        /// <param name="username">de username die je wil checken</param>
        /// <returns>of de username al bestaat is het true</returns>
        Task<bool> UsernameTaken(string username); 
        /// <summary>
        /// checkt of het telefoonnummer al bestaat in de db
        /// </summary>
        /// <param name="phoneNumber">het telefoonnummer wat je wil checken</param>
        /// <returns>als hij al bestaat returned hij true</returns>
        Task<bool> PhoneNumberTaken(string phoneNumber);
        /// <summary>
        /// checkt of het emailadress al bestaad in de db
        /// </summary>
        /// <param name="email">de email die je wil checken</param>
        /// <returns>als hij bestaat is hij true</returns>
        Task<bool> EmailTaken(string email);
    }
}