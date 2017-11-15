using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RepositoryApp.Data.Dto;
using RepositoryApp.Data.Model;

namespace RepositoryApp.Service.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUser(Guid userId);
        Task<IList<User>> GetUsers();
        Task RemoveUser(User user);
        Task<bool> UserExist(Guid userId);
        TokenModel GenerateTokenForUser(User user);
        Task RegisterUser(User user, string password);
        Task<User> FindUserByEmail(string email);
        bool AuthenticateUser(User user, string password);
        Task<bool> SaveAsync();
    }
}
