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
    public class FileService : IFileService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDirectoryService _directoryService;

        public FileService(ApplicationDbContext dbContext, IDirectoryService directoryService)
        {
            _dbContext = dbContext;
            _directoryService = directoryService;
        }

        public async Task<File> GetFileByIdAsync(Guid fileId)
        {
            var file = await _dbContext.Files.FirstOrDefaultAsync(f => f.Id == fileId);
            return file;
        }

        public async Task<IList<File>> GetFilesAsync(Guid versionId)
        {
            var files = await _dbContext.Files
                .Where(f => f.VersionId == versionId)
                .OrderBy(s => s.Name)
                .ToListAsync();
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

        public async Task<bool> RemoveDuplicatedFileAsync(List<File> files, string fileName)
        {
            var file = files.FirstOrDefault(s => s.Name == fileName);
            if (file == null)
                return false;

            DeleteFile(file);
            await SaveChangesAsync();

            _directoryService.RemoveFile(file.Path);
            return true;
        }
    }
}