using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface ITokenService
    {
        /// <summary>
        /// Gebruikt een paar geleverde klassen voor het maken van JWT token, hierin staat belangrijke informatie zoals de gebruikers AppUserId die op veel plekken wordt gebruikt om
        /// de gebruiker te identificeren
        /// </summary>
        /// <param name="appUser">het appuserobject waarvoor je een token wil maken</param>
        /// <returns>de JWT token voor de gebruiker</returns>
        string CreateToken(AppUser appUser); 
    }
}