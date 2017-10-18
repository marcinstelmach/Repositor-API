using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using RepositoryApp.Service.Services.Interfaces;

namespace RepositoryApp.Service.Services.Implementations
{
    public class DirectoryUserService : IDirectoryUserService
    {
        public async Task CreateDirectoryForUser(string path)
        {
            Directory.CreateDirectory(path);
            await Task.CompletedTask;
        }

        public async Task DeleteDirectoryForUser(string path)
        {
            Directory.Delete(path, true);
            await Task.CompletedTask;
        }
        

        public bool DirectoryForUserExist(string path)
        {
            return Directory.Exists(path);
        }
    }
}
