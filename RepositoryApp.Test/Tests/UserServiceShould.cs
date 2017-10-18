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
    public class UserServiceShould
    {
        private readonly Mock<ApplicationDbContext> _dbContext;
        private readonly Mock<IConfiguration> _configuration;
        private readonly IUserService _userService;

        public UserServiceShould()
        {
            _dbContext = new Mock<ApplicationDbContext>();
            _configuration = new Mock<IConfiguration>();
            _userService = new UserService(_dbContext.Object, _configuration.Object);
        }

        [Fact]
        public async Task ReturnUsers()
        {
            var result = await _userService.GetUsers();
            Assert.NotEmpty(result);
        }
    }
}
