using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RepositoryApp.Data.Model
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string UniqueName { get; set; }
        public IList<Repository> Repositories { get; set; }
    }
}