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
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "1234567890";
            const string specialChars = "!@#$%^&*()-_=+[]{}|<>?/";

            var length = 12;
            var password = new char[length];

            // Ensure at least one character from each required category
            password[0] = upperCase[_random.Next(upperCase.Length)];
            password[1] = lowerCase[_random.Next(lowerCase.Length)];
            password[2] = digits[_random.Next(digits.Length)];
            password[3] = specialChars[_random.Next(specialChars.Length)];

            // Fill the rest with a mix of all categories
            string allChars = upperCase + lowerCase + digits + specialChars;
            for (int i = 4; i < length; i++)
            {
                password[i] = allChars[_random.Next(allChars.Length)];
            }

            // Shuffle to avoid predictable patterns
            return new string(password.OrderBy(_ => _random.Next()).ToArray());
        }
    }
}