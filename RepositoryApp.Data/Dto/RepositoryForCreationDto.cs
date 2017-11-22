using System.ComponentModel.DataAnnotations;

namespace RepositoryApp.Data.Dto
{
    public class RepositoryForCreationDto
    {
        [Required]
        [MinLength(2)]
        [MaxLength(15)]
        public string Name { get; set; }
    }
}