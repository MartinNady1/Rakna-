using Rakna.BAL.DTO.GarageDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Rakna.BAL.Helper
{
    public class SpHelper
    {
        public bool isStringNumeric(string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsNumber(value[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public bool isValidEmail(string email)
        {
            const string pattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+[a-zA-Z]{2,}))$";
            return Regex.IsMatch(email, pattern);
        }

        public string isValidPassword(string password, int minLength = 8, int minUppercase = 1, int minLowercase = 1, int minDigit = 1, int minSpecialChar = 1)
        {
            StringBuilder errorMessage = new StringBuilder();

            if (password.Length < minLength)
            {
                errorMessage.AppendLine($"Password is too short. Minimum length is {minLength} characters.");
            }

            int uppercaseCount = 0;
            int lowercaseCount = 0;
            int digitCount = 0;
            int specialCharCount = 0;

            foreach (char ch in password)
            {
                if (char.IsUpper(ch))
                {
                    uppercaseCount++;
                }
                else if (char.IsLower(ch))
                {
                    lowercaseCount++;
                }
                else if (char.IsDigit(ch))
                {
                    digitCount++;
                }
                else if (!char.IsLetterOrDigit(ch) && !char.IsWhiteSpace(ch))
                {
                    specialCharCount++;
                }
            }

            if (uppercaseCount < minUppercase)
            {
                errorMessage.AppendLine($"Password needs at least {minUppercase} uppercase characters (A-Z).");
            }
            if (lowercaseCount < minLowercase)
            {
                errorMessage.AppendLine($"Password needs at least {minLowercase} lowercase characters (a-z).");
            }
            if (digitCount < minDigit)
            {
                errorMessage.AppendLine($"Password needs at least {minDigit} digits (0-9).");
            }
            if (specialCharCount < minSpecialChar)
            {
                errorMessage.AppendLine($"Password needs at least {minSpecialChar} special characters (e.g., !@#$%^&*).");
            }
            if (password.Contains(' '))
            {
                errorMessage.AppendLine("Password can't contain white spaces.");
            }
            return errorMessage.ToString();
        }
        public bool HasArabicCharacters(string text)
        {
            Regex regex = new Regex("[\u0623\u0628\u062C\u062F\u0631\u0633\u0635\u0637\u0639\u0641\u0642\u0644\u0645\u0646\u0647\u0648\u06CC]");
            return regex.IsMatch(text);

        }
        public bool isValidPlate(string plateCharacters, string plateNumber)
        {
            if (!HasArabicCharacters(plateCharacters)||plateCharacters.Length > 4 || plateCharacters.Length == 0 || plateNumber.Length == 0 || plateNumber.Length > 4 || !isStringNumeric(plateNumber))
                return false;

            return true;
        }

        public string isValidGarage(GarageDto garageDto)
        {
            StringBuilder response = new StringBuilder();
            // GarageName validation
            if (string.IsNullOrWhiteSpace(garageDto.GarageName))
                response.AppendLine("Garage name is required.");

            if (garageDto.TotalSpaces <= 0)
            {
                response.AppendLine("Available parking slots cannot be negative.");
            }

            // HourPrice validation
            if (garageDto.HourPrice <= 0)
            {
                response.AppendLine("Hour price must be greater than zero.");
            }

            // Street validation
            if (string.IsNullOrWhiteSpace(garageDto.street))
            {
                response.AppendLine("Street is required.");
            }

            // City validation
            if (string.IsNullOrWhiteSpace(garageDto.city))
            {
                response.AppendLine("City is required.");
            }
            return response.ToString();
        }

        public string isValidUsername(string username)
        {
            StringBuilder result = new StringBuilder();
            string pattern = @"^[a-zA-Z0-9]{5,20}$";

            if (string.IsNullOrWhiteSpace(username))
                result.AppendLine("Username cannot be empty or just white spaces.");

            if (!Regex.IsMatch(username, pattern))
                result.AppendLine("Username must be 5 to 20 characters long with no spaces and special characters.");

            return result.ToString();
        }

        public string GeneratePassword()
        {
            Random random = new Random();
            const string uppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string specialCharacters = "!@#$%^&*()_+-=[]{}|;:',.<>?";
            string password = new string(new char[] {
            uppercaseLetters[random.Next(0, uppercaseLetters.Length)],
            lowercaseLetters[random.Next(0, lowercaseLetters.Length)],
            specialCharacters[random.Next(0, specialCharacters.Length)],
            digits[random.Next(0, digits.Length)]
        });

            string allCharacters = uppercaseLetters + lowercaseLetters + specialCharacters + digits;
            while (password.Length < 8)
            {
                password += allCharacters[random.Next(0, allCharacters.Length)];
            }

            password = new string(password.ToCharArray().OrderBy(s => (random.Next(2) % 2) == 0).ToArray());

            return password;
        }
    }
}