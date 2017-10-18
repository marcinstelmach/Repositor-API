using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryApp.Service.Services.Interfaces
{
    public interface IDirectoryUserService
    {
        Task CreateDirectoryForUser(string path);
        Task DeleteDirectoryForUser(string path);
        bool DirectoryForUserExist(string path);
    }
}
