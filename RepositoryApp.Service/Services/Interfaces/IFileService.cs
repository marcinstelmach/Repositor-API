using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RepositoryApp.Data.Model;

namespace RepositoryApp.Service.Services.Interfaces
{
    public interface IFileService
    {
        Task<File> GetFileByIdAsync(Guid fileId);
        Task<IList<File>> GetFilesForVersionAsync(Guid versionId);
        Task AddFileAsync(File file);
        void DeleteFile(File file);
        Task<bool> SaveChangesAsync();
    }
}
