using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IConfiguration _configuration;
        private readonly IDirectoryService _directoryService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IMapper mapper, IUserService userService, IConfiguration configuration,
            IDirectoryService directoryService)
        {
            _mapper = mapper;
            _userService = userService;
            _configuration = configuration;
            _directoryService = directoryService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserForCreationDto userForCreationDto)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectRestult(ModelState);

            if (await _userService.FindUserByEmail(userForCreationDto.Email) != null)
                ModelState.AddModelError("email", "This email is already used");

            var user = _mapper.Map<User>(userForCreationDto);
            await _userService.RegisterUser(user, userForCreationDto.Password);
            if (!await _userService.SaveAsync())
                return StatusCode(500, "A problem with saving data");

            var path = $"{_configuration["Paths:Defaultpath"]}{user.UniqueName}";
            if (_directoryService.DirectoryExist(path))
                return StatusCode(500, "Something goes wrong...");
            try
            {
                await _directoryService.CreateDirectory(path);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Can't create directory for user..." + e.Message);
            }
            var userDto = _mapper.Map<UserForDisplayDto>(user);
            return CreatedAtRoute("GetUser", new {userId = userDto.Id}, userDto);
        }

        [AllowAnonymous]
        [HttpPost("Token")]
        public async Task<IActionResult> GetToken([FromBody] UserForLoginDto userForLogin)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userService.FindUserByEmail(userForLogin.Email);
            if (user == null)
                return BadRequest("User not Found");

            if (!_userService.AuthenticateUser(user, userForLogin.Password))
                return BadRequest("Invalid email or password");

            var token = _userService.GenerateTokenForUser(user);
            return Ok(token);
        }

        [Authorize]
        [HttpGet("{userId}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId)
                return Unauthorized();
            var user = await _userService.GetUser(userId);
            if (user == null)
                return BadRequest("User not Found");

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

        [Authorize]
        [HttpGet("GetUserInfo")]
        public IActionResult GetUserInfo()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var email = User.FindFirst(JwtRegisteredClaimNames.Iat).Value;
            return Ok(new { id = id, email = email });
        }
    }
}