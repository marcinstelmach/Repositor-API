using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryApp.Data.DAL;
using RepositoryApp.Data.Model;
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
            var versions = await _dbContext.Versions.Where(s => s.RepositoryId == repositoryId)
                .Include(s => s.Files)
                .ToListAsync();
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

        public void ChangeVersionStatusAsync(Version version)
        {
            version.ProductionVersion = !version.ProductionVersion;
            _dbContext.Versions.Update(version);
        }

        public List<File> PrepareFiles(List<File> files, string path)
        {
            var preparedFiles = files.Select(file => new File
                {
                    Name = file.Name,
                    ContentType = file.ContentType,
                    CreationDateTime = DateTime.Now,
                    UniqueName = file.UniqueName,
                    Path = path
                })
                .ToList();
            return preparedFiles;
        }

        public async Task<Version> GetVersionWithFilesAsync(Guid versionId)
        {
            var version = await _dbContext.Versions.Where(s => s.Id == versionId).Include(f => f.Files)
                .FirstOrDefaultAsync();
            return version;
        }
    }
}
