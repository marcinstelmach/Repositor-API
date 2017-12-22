using System;

namespace RepositoryApp.Data.Dto
{
    public class VersionForDisplay
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDateTime { get; set; }
        public bool ProductionVersion { get; set; }
        public int CountOfFiles { get; set; }
        public Guid RepositoryId { get; set; }
    }
}