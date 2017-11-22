using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryApp.Data.DAL;
using RepositoryApp.Service.Services.Interfaces;
using Version = RepositoryApp.Data.Model.Version;

namespace RepositoryApp.Service.Services.Implementations
{
    public class VersionService : IVersionService
    {
        private readonly ApplicationDbContext _dbContext;

        public VersionService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Version> GetVersionByIdAsync(Guid versionId)
        {
            var version = await _dbContext.Versions.FirstOrDefaultAsync(s => s.Id == versionId);
            return version;
        }

        public async Task<IList<Version>> GetVersionsForUserAsync(Guid repositoryId)
        {
            var versions = await _dbContext.Versions.Where(s => s.RepositoryId == repositoryId).ToListAsync();
            return versions;
        }

        public async Task AddVersionAsync(Version version)
        {
            await _dbContext.Versions.AddAsync(version);
        }

        public void DeleteVersion(Version version)
        {
            _dbContext.Versions.Remove(version);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() >= 0;
        }
    }
}
