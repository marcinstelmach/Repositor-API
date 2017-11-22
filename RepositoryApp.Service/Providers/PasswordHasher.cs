using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace RepositoryApp.Service.Providers
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password, byte[] salt)
        {
            var passwordHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA1,
                10000,
                64));
            return passwordHashed;
        }
    }
}