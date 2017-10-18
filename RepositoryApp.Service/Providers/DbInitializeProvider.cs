using System;
using System.Collections.Generic;
using System.Linq;
using RepositoryApp.Data.DAL;
using RepositoryApp.Data.Model;
using RepositoryApp.Service.Helpers;
using Version = RepositoryApp.Data.Model.Version;

namespace RepositoryApp.Service.Providers
{
    public class DbInitializeProvider
    {
        public static void InitializeWithDefaults(ApplicationDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            if (dbContext.Users.Any())
                return;
            var random = string.Empty;
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "test@gmail.com",
                    UserName = "tescik",
                    EmailConfirmed = true,
                    PasswordHash =
                        "AQAAAAEAACcQAAAAEMtt9h4GOrsKAmzphBrm+Uk4ZJpwlwouaMW8dQD2oHet7f5BIRZPYfgu3ZoR2m1Wdg==",
                    NormalizedEmail = "TEST@GMAIL.COM",
                    NormalizedUserName = "TEST@GMAIL.COM",
                    UniqueName = $"tescik{random.RandomString(10)}",
                    LockoutEnabled = true,
                    FirstName = "Jan",
                    LastName = "Kowalski",
                    CreationDateTime = DateTime.Now,
                    Repositories = new List<Repository>
                    {
                        new Repository
                        {
                            Id = Guid.NewGuid(),
                            Name = "MyRepository1",
                            UniqueName = $"MyRepository1_{random.RandomString(10)}",
                            CreationDateTime = DateTime.Now,
                            ModifyDateTime = DateTime.Now,
                            Versions = new List<Version>
                            {
                                new Version
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "1",
                                    UniqueName = $"1_{random.RandomString(10)}",
                                    Description = "For Test",
                                    CreationDateTime = DateTime.Now,
                                    ModifDateTime = DateTime.Now,
                                    RepositoryId = null,
                                    Files = new List<File>
                                    {
                                        new File
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "FileOne",
                                            UniqueName = $"FileOne_{random.RandomString(10)}",
                                            CreationDateTime = DateTime.Now
                                        },
                                        new File
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "FileTwo",
                                            UniqueName = $"FileTwo_{random.RandomString(10)}",
                                            CreationDateTime = DateTime.Now
                                        }
                                    }
                                },
                                new Version
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "2",
                                    UniqueName = $"2_{random.RandomString(10)}",
                                    Description = "For Test",
                                    CreationDateTime = DateTime.Now,
                                    ModifDateTime = DateTime.Now,
                                    RepositoryId = null,
                                    Files = new List<File>
                                    {
                                        new File
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "FileOne",
                                            UniqueName = $"FileOne_{random.RandomString(10)}",
                                            CreationDateTime = DateTime.Now
                                        },
                                        new File
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "FileTwo",
                                            UniqueName = $"FileTwo_{random.RandomString(10)}",
                                            CreationDateTime = DateTime.Now
                                        },
                                        new File
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "FileThree",
                                            UniqueName = $"FileThree_{random.RandomString(10)}",
                                            CreationDateTime = DateTime.Now
                                        }
                                    }
                                }
                            }
                        },
                        new Repository
                        {
                            Id = Guid.NewGuid(),
                            Name = "MyRepository2",
                            UniqueName = $"MyRepository2_{random.RandomString(10)}",
                            CreationDateTime = DateTime.Now,
                            ModifyDateTime = DateTime.Now,
                            Versions = new List<Version>
                            {
                                new Version
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "1",
                                    UniqueName = $"1_{random.RandomString(10)}",
                                    Description = "For Test",
                                    CreationDateTime = DateTime.Now,
                                    ModifDateTime = DateTime.Now,
                                    RepositoryId = null,
                                    Files = new List<File>
                                    {
                                        new File
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "FileOne",
                                            UniqueName = $"FileOne_{random.RandomString(10)}",
                                            CreationDateTime = DateTime.Now
                                        },
                                        new File
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "FileTwo",
                                            UniqueName = $"FileTwo_{random.RandomString(10)}",
                                            CreationDateTime = DateTime.Now
                                        }
                                    }
                                },
                                new Version
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "2",
                                    UniqueName = $"2_{random.RandomString(10)}",
                                    Description = "For Test",
                                    CreationDateTime = DateTime.Now,
                                    ModifDateTime = DateTime.Now,
                                    RepositoryId = null,
                                    Files = new List<File>
                                    {
                                        new File
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "FileOne",
                                            UniqueName = $"FileOne_{random.RandomString(10)}",
                                            CreationDateTime = DateTime.Now
                                        },
                                        new File
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "FileTwo",
                                            UniqueName = $"FileTwo_{random.RandomString(10)}",
                                            CreationDateTime = DateTime.Now
                                        },
                                        new File
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "FileThree",
                                            UniqueName = $"FileThree_{random.RandomString(10)}",
                                            CreationDateTime = DateTime.Now
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();
        }
    }
}