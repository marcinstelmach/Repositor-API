using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace RepositoryApp.Service.Providers
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password, byte[] salt)
        {
            var passwordHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 64));
            return passwordHashed;
        }
    }
}
