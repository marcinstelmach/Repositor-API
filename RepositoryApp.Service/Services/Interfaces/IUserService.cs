using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
        Task<bool> SaveAsync();
    }
}
