using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using RepositoryApp.Data.Model;

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
