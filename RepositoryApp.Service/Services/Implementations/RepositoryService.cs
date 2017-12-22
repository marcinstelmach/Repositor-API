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

        public async Task CreateRepositoryAsync(Guid userId, Repository repository)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == userId);
            if (user == null)
                repository.Id = Guid.NewGuid();
            user.Repositories = new List<Repository>();
            user.Repositories.Add(repository);
        }

        public async Task<bool> RepositoryExistAsync(Guid repositoryId)
        {
            var repository = await _dbContext.Repositories.FirstOrDefaultAsync(s => s.Id == repositoryId);
            return repository == null;
        }

        public async Task RemoveRepositoryAsync(Repository repository)
        {
            _dbContext.Repositories.Remove(repository);
            await Task.CompletedTask;
        }

        public async Task<Repository> GetRepositoryAsync(Guid userId, Guid repositoryId)
        {
            var repository =
                await _dbContext.Repositories.FirstOrDefaultAsync(s => s.UserId == userId && s.Id == repositoryId);
            return repository;
        }

        public async Task<IList<Repository>> GetRepositoriesAsync(Guid userId)
        {
            var repository =
                await _dbContext.Repositories
                    .Where(s => s.UserId == userId)
                    .OrderByDescending(s => s.CreationDateTime)
                    .Include(s => s.Versions)
                    .ToListAsync();
            return repository;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() >= 0;
        }
    }
}