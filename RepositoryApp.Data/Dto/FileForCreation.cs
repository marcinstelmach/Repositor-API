using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryApp.Data.Dto
{
    public class FileForCreation
    {
        [Required]
        public string Name { get; set; }
    }
}
