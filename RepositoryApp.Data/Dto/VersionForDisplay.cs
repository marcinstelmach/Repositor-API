using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using RepositoryApp.Data.Model;

namespace RepositoryApp.Data.Dto
{
    public class VersionForDisplay
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime ModifDateTime { get; set; }
        public Guid RepositoryId { get; set; }
    }
}
