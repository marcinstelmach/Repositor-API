using System.IO;
using System.Threading.Tasks;
using RepositoryApp.Service.Services.Interfaces;

namespace RepositoryApp.Service.Services.Implementations
{
    public class DirectoryService : IDirectoryService
    {
        public async Task CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
            await Task.CompletedTask;
        }

        public bool DirectoryExist(string path)
        {
            return Directory.Exists(path);
        }

        public async Task RemoveDirectory(string path)
        {
            Directory.Delete(path, true);
            await Task.CompletedTask;
        }

        public async Task RenameDirectory(string path, string newName)
        {
            var newPath = path.Substring(0, path.LastIndexOf('\\')) + path.Substring(path.LastIndexOf('\\')) + newName;
            Directory.Move(path, newPath);
            await Task.CompletedTask;
        }
    }
}