using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryApp.Data.DAL;
using RepositoryApp.Data.Model;
using RepositoryApp.Service.Providers;
using RepositoryApp.Service.Services.Interfaces;

namespace RepositoryApp.Service.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<User> GetUser(Guid userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == userId);
        }

        public async Task<IList<User>> GetUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task RemoveUser(User user)
        {
            _dbContext.Users.Remove(user);
            await Task.CompletedTask;
        }

        public async Task<bool> UserExist(Guid userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == userId) != null;
        }

        public TokenModel GenerateTokenForUser(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
                _configuration["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var resultToken = new TokenModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ValidTo = token.ValidTo,
                Email = user.Email,
                UserId = user.Id.ToString()
            };

            return resultToken;
        }

        public async Task RegisterUser(User user, string password)
        {
            new RNGCryptoServiceProvider().GetBytes(user.Salt = new byte[32]);
            user.PasswordHash = PasswordHasher.HashPassword(password, user.Salt);
            user.CreationDateTime = DateTime.Now;

            await _dbContext.Users.AddAsync(user);
        }

        public async Task<User> FindUserByEmail(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public bool AuthenticateUser(User user, string password)
        {
            var passwordHash = PasswordHasher.HashPassword(password, user.Salt);
            return user.PasswordHash.Equals(passwordHash);
        }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() >= 0;
        }
    }
}