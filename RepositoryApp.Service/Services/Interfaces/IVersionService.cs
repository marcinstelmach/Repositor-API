using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
    }
}
