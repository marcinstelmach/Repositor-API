using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RepositoryApp.Data.Model;

namespace RepositoryApp.Service.Services.Interfaces
{
    public interface IRepositoryService
    {
        Task CreateRepositoryForUser(Guid userId, Repository repository);
        Task<bool> RepositoryExist(Guid repositoryId);
        Task RemoveRepository(Repository repository);
        Task<Repository> GetRepositoryForUser(Guid userId, Guid repositoryId);
        Task<IList<Repository>> GetRepositoriesForUser(Guid userId);
        Task<bool> SaveAsync();
    }
}