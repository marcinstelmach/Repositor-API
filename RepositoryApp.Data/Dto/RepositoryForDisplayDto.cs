using System;

namespace RepositoryApp.Data.Dto
{
    public class RepositoryForDisplayDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime ModifyDateTime { get; set; }
        public int CountOfVersion { get; set; }
        public Guid UserId { get; set; }
    }
}