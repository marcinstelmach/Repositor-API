using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryApp.Data.DAL;
using RepositoryApp.Data.Model;
using RepositoryApp.Service.Services.Interfaces;

namespace RepositoryApp.Service.Services.Implementations
{
    public class RepositoryService : IRepositoryService
    {
        private readonly ApplicationDbContext _dbContext;

        public RepositoryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateRepositoryForUser(Guid userId, Repository repository)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == userId);
            if (user == null)
                repository.Id = Guid.NewGuid();
            user.Repositories = new List<Repository>();
            user.Repositories.Add(repository);
        }

        public async Task<bool> RepositoryExist(Guid repositoryId)
        {
            var repository = await _dbContext.Repositories.FirstOrDefaultAsync(s => s.Id == repositoryId);
            return repository == null;
        }

        public async Task RemoveRepository(Repository repository)
        {
            _dbContext.Repositories.Remove(repository);
            await Task.CompletedTask;
        }

        public async Task<Repository> GetRepositoryForUser(Guid userId, Guid repositoryId)
        {
            var repository =
                await _dbContext.Repositories.FirstOrDefaultAsync(s => s.UserId == userId && s.Id == repositoryId);
            return repository;
        }

        public async Task<IList<Repository>> GetRepositoriesForUser(Guid userId)
        {
            var repository =
                await _dbContext.Repositories.Where(s => s.UserId == userId).ToListAsync();
            return repository;
        }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() >= 0;
        }
    }
}