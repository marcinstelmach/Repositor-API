using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RepositoryApp.Data.Model;

namespace RepositoryApp.Service.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserAsync(Guid userId);
        Task<IList<User>> GetUsersAsync();
        Task RemoveUserAsync(User user);
        Task<bool> UserExistAsync(Guid userId);
        TokenModel GenerateTokenForUser(User user);
        Task RegisterUserAsync(User user, string password);
        Task<User> FindUserByEmailAsync(string email);
        bool AuthenticateUser(User user, string password);
        Task<bool> SaveChangesAsync();
    }
}