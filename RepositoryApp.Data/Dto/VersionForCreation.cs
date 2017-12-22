using System.ComponentModel.DataAnnotations;

namespace RepositoryApp.Data.Dto
{
    public class VersionForCreation
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        public string Description { get; set; }
    }
}