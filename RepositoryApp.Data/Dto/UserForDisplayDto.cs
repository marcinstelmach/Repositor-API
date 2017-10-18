using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryApp.Data.Dto
{
    public class UserForDisplayDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
