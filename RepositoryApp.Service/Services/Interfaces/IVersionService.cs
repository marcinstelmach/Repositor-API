using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RepositoryApp.Data.Model;
using Version = RepositoryApp.Data.Model.Version;

namespace RepositoryApp.Service.Services.Interfaces
{
    public interface IVersionService
    {
        Task<Version> GetVersionByIdAsync(Guid versionId);
        Task<IList<Version>> GetVersionsWithFilesAsync(Guid repositoryId);
        Task AddVersionAsync(Version version);
        void DeleteVersion(Version version);
        Task<bool> SaveChangesAsync();
        void ChangeVersionStatus(Version version);
        List<File> PrepareFiles(List<File> files, string path);
        Task<Version> GetVersionWithFilesAsync(Guid versionId);
        Task SetAllAsNonProductionVersionAsync(Guid repositoryId);
    }
}