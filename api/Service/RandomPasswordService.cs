using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Service
{
    public class RandomPasswordService
    {
        private static readonly Random _random = new();

        public static string GenerateRandomPassword()
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"; 
            const string digits = "1234567890"; 
            const string specialChars = "!@#$%^&*()-_=+[]{}|;:'\",.<>?/";  
            var length = 12;
            var password = new char[length];

            password[0] = digits[_random.Next(digits.Length)];
            password[1] = specialChars[_random.Next(specialChars.Length)];

            for (int i = 2; i < length; i++)
            {
                var charSet = validChars + digits + specialChars;
                password[i] = charSet[_random.Next(charSet.Length)];
            }

            return new string(password);
        }
    }
}