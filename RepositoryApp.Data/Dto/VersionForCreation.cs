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
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
