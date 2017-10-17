using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryApp.Data.Model
{
    public class Repository
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string UniqueName { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime ModifyDateTime { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }

        public User User { get; set; }
        public IList<Version> Versions { get; set; }
    }
}