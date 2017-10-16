using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace RepositoryApp.Data.Model
{
    public class User : IdentityUser<Guid>
    {
        [Key]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreationDateTime { get; set; }
        public IList<Repository> Repositories { get; set; }
    }
}
