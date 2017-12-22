using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RepositoryApp.Data.Model;

namespace RepositoryApp.Service.Services.Interfaces
{
    public interface IFileService
    {
        Task<File> GetFileByIdAsync(Guid fileId);
        Task<IList<File>> GetFilesAsync(Guid versionId);
        Task AddFileAsync(File file);
        void DeleteFile(File file);
        Task<bool> SaveChangesAsync();
        Task<bool> RemoveDuplicatedFileAsync(List<File> files, string fileName);
    }
}