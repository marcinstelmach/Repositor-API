using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using RepositoryApp.Data.DAL;
using RepositoryApp.Data.Model;
using RepositoryApp.Service.Services.Implementations;
using RepositoryApp.Service.Services.Interfaces;
using Xunit;

namespace RepositoryApp.Test.Tests
{
    public class UserServiceTest
    {
        private readonly Mock<ApplicationDbContext> _dbContext;
        private readonly Mock<IConfiguration> _configuration;
        private readonly IUserService _userService;


    }
}
