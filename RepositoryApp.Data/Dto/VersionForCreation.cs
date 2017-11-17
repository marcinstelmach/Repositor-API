using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using RepositoryApp.Data.Model;

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
