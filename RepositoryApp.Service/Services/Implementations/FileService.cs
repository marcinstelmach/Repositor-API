using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryApp.Data.DAL;
using RepositoryApp.Data.Model;
using RepositoryApp.Service.Services.Interfaces;

namespace RepositoryApp.Service.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly ApplicationDbContext _dbContext;

        public FileService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<File> GetFileByIdAsync(Guid fileId)
        {
            var file = await _dbContext.Files.FirstOrDefaultAsync(f => f.Id == fileId);
            return file;
        }

        public async Task<IList<File>> GetFilesForVersionAsync(Guid versionId)
        {
            var files = await _dbContext.Files.Where(f => f.Id == versionId).ToListAsync();
            return files;
        }

        public async Task AddFileAsync(File file)
        {
            await _dbContext.AddAsync(file);
        }

        public void DeleteFile(File file)
        {
            _dbContext.Files.Remove(file);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() >= 0;
        }
    }
}
