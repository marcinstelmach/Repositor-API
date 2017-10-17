using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using RepositoryApp.Data.Model;
using Version = System.Version;

namespace RepositoryApp.Data.Dto
{
    public class RepositoryForDisplayDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime ModifyDateTime { get; set; }
        public Guid UserId { get; set; }
    }
}
