using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryApp.Service.Services.Interfaces
{
    public interface IVersionService
    {
        Task<Version> GetVersionByIdAsync(Guid versionId);
        Task<IList<Version>> GetVersionsForUserAsync(Guid userId);
        Task AddVersionAsync(Version version);
        Task DeleteVersion(Version version);
        Task<bool> SaveChangesAsync();
    }
}
