using System.ComponentModel.DataAnnotations;

namespace RepositoryApp.Data.Dto
{
    public class UserForLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}