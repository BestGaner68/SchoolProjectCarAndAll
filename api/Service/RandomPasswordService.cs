using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Service
{
    /// <summary>
    /// niet een hele mooie methode maar generate een random password voor nieuwe wagenparkbeheerder, gebruiker worden gepromt om gelijk hun password te veranderen
    /// </summary>
    public class RandomPasswordService
    {
        private static readonly Random _random = new();

        public static string GenerateRandomPassword()
        {
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "1234567890";
            const string specialChars = "!@#$%^&*()-_=+[]{}|<>?/";
            var length = 12;
            var password = new char[length];

            password[0] = upperCase[_random.Next(upperCase.Length)];
            password[1] = lowerCase[_random.Next(lowerCase.Length)];
            password[2] = digits[_random.Next(digits.Length)];
            password[3] = specialChars[_random.Next(specialChars.Length)];
            string allChars = upperCase + lowerCase + digits + specialChars;
            for (int i = 4; i < length; i++)
            {
                password[i] = allChars[_random.Next(allChars.Length)];
            }
            return new string(password.OrderBy(_ => _random.Next()).ToArray());
        }
    }
}