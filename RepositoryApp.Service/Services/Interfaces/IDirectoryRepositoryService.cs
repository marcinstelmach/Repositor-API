using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryApp.Service.Services.Interfaces
{
    public interface IDirectoryRepositoryService
    {
        Task CreateDirectoryRepository(string path);
        bool DirectoryRepositoryExist(string path);
        Task RemoveDirectoryRepository(string path);
        Task RenameDirectoryRepository(string path, string newName);
    }
}
