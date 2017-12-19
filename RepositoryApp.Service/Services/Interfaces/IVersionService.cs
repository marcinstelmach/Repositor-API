using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RepositoryApp.Data.Model;
using Version = RepositoryApp.Data.Model.Version;

namespace RepositoryApp.Service.Services.Interfaces
{
    public interface IVersionService
    {
        Task<Version> GetVersionByIdAsync(Guid versionId);
        Task<IList<Version>> GetVersionsForUserAsync(Guid repositoryId);
        Task AddVersionAsync(Version version);
        void DeleteVersion(Version version);
        Task<bool> SaveChangesAsync();
        void ChangeVersionStatusAsync(Version version);
        List<File> PrepareFiles(List<File> files, string path);
        Task<Version> GetVersionWithFilesAsync(Guid versionId);
    }
}
