using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryApp.Data.Model
{
    public class File
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string UniqueName { get; set; }
        public DateTime CreationDateTime { get; set; }

        [ForeignKey("Version")]
        public Guid VersionId { get; set; }

        public Version Version { get; set; }
    }
}