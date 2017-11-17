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
    [Authorize]
    [Route("api/users/{userId}/repositories/")]
    public class RepositoryController : Controller
    {
        private readonly IDirectoryService _directoryService;
        private readonly IMapper _mapper;
        private readonly IRepositoryService _repositoryService;
        private readonly IUserService _userService;

        public RepositoryController(IRepositoryService repositoryService,
            IDirectoryService directoryService,
            IMapper mapper,
            IUserService userService)
        {
            _repositoryService = repositoryService;
            _directoryService = directoryService;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRepositoriesForUser(Guid userId)
        {
            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId)
                return Unauthorized();
            if (!await _userService.UserExist(userId))
                return BadRequest("User not found");

            var repositories = await _repositoryService.GetRepositoriesForUser(userId);
            if (repositories == null)
            {
                return BadRequest();
            }
            var repositoryForDisplay = _mapper.Map<IList<RepositoryForDisplayDto>>(repositories);
            return Ok(repositoryForDisplay);
        }

        [HttpGet("{repositoryId}", Name = "GetRepository")]
        public async Task<IActionResult> GetRepositories(Guid userId, Guid repositoryId)
        {
            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId)
                return Unauthorized();

            if (!await _userService.UserExist(userId))
                return BadRequest("User not found");

            var repository = await _repositoryService.GetRepositoryForUser(userId, repositoryId);
            if (repository == null)
            {
                return BadRequest();
            }
            var repositoryForDisplay = _mapper.Map<RepositoryForDisplayDto>(repository);
            return Ok(repositoryForDisplay);
        }

        [HttpPost]
        public async Task<IActionResult> AddRepositoryForUser(Guid userId,
            [FromBody] RepositoryForCreationDto creationDto)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectRestult(ModelState);

            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId)
                return Unauthorized();

            var user = await _userService.GetUser(userId);
            if (user == null)
                return BadRequest("User not found");

            var repository = _mapper.Map<Repository>(creationDto);
            repository.Path = $"{user.Path}{repository.UniqueName}\\";
            user.Repositories = new List<Repository>
            {
                repository
            };
            if (!await _repositoryService.SaveAsync())
                return StatusCode(500, "Fault while save in database");


            if (_directoryService.DirectoryExist(repository.Path))
                return StatusCode(500, "Something goes wrong");

            try
            {
                await _directoryService.CreateDirectory(repository.Path);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Something goes wrong, " + e.Message);
            }

            var repositoryDto = _mapper.Map<RepositoryForDisplayDto>(repository);
            return CreatedAtRoute("GetRepository", new {userId = repository.UserId, repositoryId = repository.Id},
                repositoryDto);
        }

        [HttpDelete("{repositoryId}")]
        public async Task<IActionResult> DeleteRepository(Guid userId, Guid repositoryId)
        {
            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId)
                return Unauthorized();

            var repository = await _repositoryService.GetRepositoryForUser(userId, repositoryId);
            if (repository == null)
                return BadRequest($"Repository {repositoryId} doesn't exist");

            await _repositoryService.RemoveRepository(repository);

            try
            {
                await _directoryService.RemoveDirectory(repository.Path);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

            if (!await _repositoryService.SaveAsync())
                return StatusCode(500, "Fail while saving database");

            return NoContent();
        }
    }
}