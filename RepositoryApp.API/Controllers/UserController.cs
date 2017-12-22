using System;
using System.Collections.Generic;
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
            if (await _userService.FindUserByEmailAsync(userForCreationDto.Email) != null)
                ModelState.AddModelError("email", "This email is already used");

            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectRestult(ModelState);

            var user = _mapper.Map<User>(userForCreationDto);
            user.Path = $"{_configuration["Paths:Defaultpath"]}{user.UniqueName}\\";
            await _userService.RegisterUserAsync(user, userForCreationDto.Password);
            if (!await _userService.SaveChangesAsync())
                return StatusCode(500, "A problem with saving data");


            if (_directoryService.DirectoryExist(user.Path))
                return StatusCode(500, "Something goes wrong...");
            try
            {
                await _directoryService.CreateDirectory(user.Path);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Can't create directory for user..." + e.Message);
            }
            var userDto = _mapper.Map<UserForDisplayDto>(user);
            return CreatedAtRoute("GetUserAsync", new {userId = userDto.Id}, userDto);
        }

        [AllowAnonymous]
        [HttpPost("Token")]
        public async Task<IActionResult> GetToken([FromBody] UserForLoginDto userForLogin)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userService.FindUserByEmailAsync(userForLogin.Email);
            if (user == null)
                return BadRequest("User not Found");

            if (!_userService.AuthenticateUser(user, userForLogin.Password))
                return BadRequest("Invalid email or password");

            var token = _userService.GenerateTokenForUser(user);
            return Ok(token);
        }

        [Authorize]
        [HttpGet("{userId}", Name = "GetUserAsync")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId)
                return Unauthorized();
            var user = await _userService.GetUserAsync(userId);
            if (user == null)
                return BadRequest("User not Found");

            var userDto = _mapper.Map<UserForDisplayDto>(user);
            return Ok(userDto);
        }


        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            var usersDto = _mapper.Map<IList<UserForDisplayDto>>(users);
            return Ok(usersDto);
        }

        [Authorize]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId)
                return Unauthorized();

            var user = await _userService.GetUserAsync(userId);
            if (user == null)
                return BadRequest();

            try
            {
                await _directoryService.RemoveDirectory(user.Path);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }


            await _userService.RemoveUserAsync(user);
            if (!await _userService.SaveChangesAsync())
                return StatusCode(500, "Fault while saving database");

            return NoContent();
        }
    }
}