using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryApp.Data.Dto
{
    public class FileForDisplay
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDateTime { get; set; }
        public Guid VersionId { get; set; }
    }
}
