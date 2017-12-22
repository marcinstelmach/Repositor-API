using System;

namespace RepositoryApp.Data.Dto
{
    public class FileForDisplay
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CreationDateTime { get; set; }
        public bool Overrided { get; set; }
        public Guid VersionId { get; set; }
    }
}