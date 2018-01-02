using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryApp.Data.Dto;
using RepositoryApp.Service.Helpers;
using RepositoryApp.Service.Services.Interfaces;
using Version = RepositoryApp.Data.Model.Version;

namespace RepositoryApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/repositories/{repositoryId}/versions/")]
    public class VersionController : Controller
    {
        private readonly IDirectoryService _directoryService;
        private readonly IMapper _mapper;
        private readonly IRepositoryService _repositoryService;
        private readonly IVersionService _versionService;

        public VersionController(IDirectoryService directoryService, IMapper mapper, IVersionService versionService,
            IRepositoryService repositoryService)
        {
            _directoryService = directoryService;
            _mapper = mapper;
            _versionService = versionService;
            _repositoryService = repositoryService;
        }


        [HttpGet]
        public async Task<IActionResult> GetVersions(Guid userId, Guid repositoryId)
        {
            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var repository = await _repositoryService.GetRepositoryAsync(userId, repositoryId);
            if (currentUserId != userId || repository == null || repository.UserId != userId)
                return BadRequest();

            var versions = await _versionService.GetVersionsWithFilesAsync(repositoryId);
            var versionsDto = _mapper.Map<IList<VersionForDisplay>>(versions);
            return Ok(versionsDto);
        }

        [HttpGet("{versionId}", Name = "GetVersion")]
        public async Task<IActionResult> GetVersion(Guid userId, Guid repositoryId, Guid versionId)
        {
            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var repository = await _repositoryService.GetRepositoryAsync(userId, repositoryId);
            if (currentUserId != userId || repository == null || repository.UserId != userId)
                return BadRequest();

            var version = await _versionService.GetVersionWithFilesAsync(versionId);
            if (version == null)
                return BadRequest();
            var versionDto = _mapper.Map<VersionForDisplay>(version);
            return Ok(versionDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddVersion(Guid userId, Guid repositoryId,
            [FromBody] VersionForCreation versionDto)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectRestult(ModelState);
            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (userId != currentUserId)
                return Unauthorized();

            var repository = await _repositoryService.GetRepositoryAsync(userId, repositoryId);
            if (repository == null)
                return BadRequest();

            var version = _mapper.Map<Version>(versionDto);
            version.Path = $"{repository.Path}{version.UniqueName}\\";
            version.CreationDateTime = DateTime.Now;
            repository.Versions = new List<Version>
            {
                version
            };

            if (!await _versionService.SaveChangesAsync())
                return StatusCode(500, "Fault while save in database");

            if (_directoryService.DirectoryExist(version.Path))
                return StatusCode(500, "Something goes wrong");

            try
            {
                await _directoryService.CreateDirectory(version.Path);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Something goes wrong, " + e.Message);
            }

            var versionForDisplay = _mapper.Map<VersionForDisplay>(version);
            return CreatedAtRoute("GetVersion",
                new {userId, repositoryId, versionId = version.Id}, versionForDisplay);
        }

        [HttpDelete("{versionId}")]
        public async Task<IActionResult> DeleteVersion(Guid userId, Guid repositoryId, Guid versionId)
        {
            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (userId != currentUserId)
                return Unauthorized();

            var version = await _versionService.GetVersionByIdAsync(versionId);
            if (version == null)
                return BadRequest();

            _versionService.DeleteVersion(version);
            if (!await _versionService.SaveChangesAsync())
                return StatusCode(500, "failed while saving in database");

            if (!_directoryService.DirectoryExist(version.Path))
                return StatusCode(500, "Directory for version doesn't exist ! :O");
            try
            {
                await _directoryService.RemoveDirectory(version.Path);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
            return NoContent();
        }

        [HttpPut("{versionId}")]
        public async Task<IActionResult> ChangeVersionStatus(Guid userId, Guid repositoryId, Guid versionId)
        {
            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (userId != currentUserId)
                return Unauthorized();

            var version = await _versionService.GetVersionByIdAsync(versionId);
            if (version == null)
                return BadRequest();
            await _versionService.SetAllAsNonProductionVersionAsync(repositoryId);
            _versionService.ChangeVersionStatus(version);
            if (!await _versionService.SaveChangesAsync())
                return StatusCode(500, "failed while saving in database");
            var versionDto = _mapper.Map<VersionForDisplay>(version);
            return Ok(versionDto);
        }

        [HttpPost("{versionId}")]
        public async Task<IActionResult> AddOverVersion(Guid userId, Guid repositoryId, Guid versionId,
            [FromBody] VersionForCreation versionForCreation)
        {
            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (userId != currentUserId)
                return Unauthorized();
            var repository = await _repositoryService.GetRepositoryAsync(userId, repositoryId);
            var baseVersion = await _versionService.GetVersionWithFilesAsync(versionId);
            if (baseVersion == null)
                return BadRequest();

            var version = _mapper.Map<Version>(versionForCreation);
            version.Path = $"{repository.Path}\\{version.UniqueName}\\";
            version.Files = _versionService.PrepareFiles(baseVersion.Files, version.Path);
            version.CreationDateTime = DateTime.Now;

            repository.Versions.Add(version);

            if (!await _versionService.SaveChangesAsync())
                return StatusCode(500, "Error while saving");
            if (_directoryService.DirectoryExist(version.Path))
                return StatusCode(500, "Something goes wrong");

            try
            {
                await _directoryService.CreateDirectory(version.Path);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Something goes wrong, " + e.Message);
            }

            var fileNames = version.Files.Select(s => s.Name).ToList();

            _directoryService.MoveFiles(baseVersion.Path, version.Path, fileNames);

            var versionForDisplay = _mapper.Map<VersionForDisplay>(version);
            return CreatedAtRoute("GetVersion",
                new {userId, repositoryId, versionId = version.Id}, versionForDisplay);
        }
    }
}