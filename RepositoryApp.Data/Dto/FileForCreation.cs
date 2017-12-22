using System.ComponentModel.DataAnnotations;

namespace RepositoryApp.Data.Dto
{
    public class FileForCreation
    {
        [Required]
        public string Name { get; set; }
    }
}