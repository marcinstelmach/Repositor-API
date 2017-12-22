using Microsoft.EntityFrameworkCore;
using RepositoryApp.Data.Model;

namespace RepositoryApp.Data.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<Version> Versions { get; set; }
        public DbSet<File> Files { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("User").Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Entity<Repository>().ToTable("Repository");
            builder.Entity<Version>().ToTable("Version");
            builder.Entity<File>().ToTable("File").Property(s => s.Overrided).HasDefaultValue(false);
        }
    }
}