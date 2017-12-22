using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RepositoryApp.Data.Model;

namespace RepositoryApp.Service.Services.Interfaces
{
    public interface IRepositoryService
    {
        Task CreateRepositoryAsync(Guid userId, Repository repository);
        Task<bool> RepositoryExistAsync(Guid repositoryId);
        Task RemoveRepositoryAsync(Repository repository);
        Task<Repository> GetRepositoryAsync(Guid userId, Guid repositoryId);
        Task<IList<Repository>> GetRepositoriesAsync(Guid userId);
        Task<bool> SaveChangesAsync();
    }
}