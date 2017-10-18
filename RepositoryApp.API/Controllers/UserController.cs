using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.Configuration;
using RepositoryApp.Data.Dto;
using RepositoryApp.Data.Model;
using RepositoryApp.Service.Helpers;
using RepositoryApp.Service.Services.Interfaces;

namespace RepositoryApp.API.Controllers
{
    [Route("api/Users")]
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _accessor;
        private readonly IConfiguration _configuration;
        private readonly IDirectoryUserService _directoryUserService;

        public UserController(IMapper mapper, SignInManager<User> signInManager, UserManager<User> userManager, IUserService userService, IHttpContextAccessor accessor, IConfiguration configuration,
            IDirectoryUserService directoryUserService)
        {
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _userService = userService;
            _accessor = accessor;
            _configuration = configuration;
            _directoryUserService = directoryUserService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserForCreationDto userForCreationDto)
        {
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectRestult(ModelState);
            }
            var random = string.Empty;
            var user = _mapper.Map<User>(userForCreationDto);
            user.CreationDateTime = DateTime.Now;
            user.EmailConfirmed = true;
            user.UniqueName = $"{user.UserName}_{random.RandomString(10)}";
            var result = await _userManager.CreateAsync(user, userForCreationDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var path = $"{_configuration["Paths:Defaultpath"]}{user.UniqueName}";
            if (_directoryUserService.DirectoryForUserExist(path))
            {
                return StatusCode(500, "Something goes wrong...");
            }
            try
            {
                await _directoryUserService.CreateDirectoryForUser(path);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Can't create directory for user..."+e.Message);
            }
            var userDto = _mapper.Map<UserForDisplayDto>(user);
            return CreatedAtRoute("GetUser", new {userId = userDto.Id}, userDto);
        }

        [Authorize]
        [HttpGet("{userId}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            var currentUserId = _accessor.CurrentUser();
            if (currentUserId!=userId)
            {
                return Unauthorized();
            }
            var user = await _userService.GetUser(userId);
            if (user == null)
            {
                return BadRequest("User not Found");
            }

            var userDto = _mapper.Map<UserForDisplayDto>(user);
            return Ok(userDto);
        }


        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();
            var usersDto = _mapper.Map<IList<UserForDisplayDto>>(users);
            return Ok(usersDto);
        }

        [AllowAnonymous]
        [HttpPost("Token")]
        public async Task<IActionResult> GetToken([FromBody] UserForLoginDto userForLogin)
        {
            var user = await _userManager.FindByEmailAsync(userForLogin.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var token = _userService.GenerateTokenForUser(user);
            return Ok(token);
        }

        [Authorize]
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}