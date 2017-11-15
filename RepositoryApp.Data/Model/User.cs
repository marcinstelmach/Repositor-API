using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RepositoryApp.Data.Model
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public byte[] Salt { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string UniqueName { get; set; }
        public IList<Repository> Repositories { get; set; }
    }
}