using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryApp.Data.Dto;
using RepositoryApp.Service.Services.Interfaces;

namespace RepositoryApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/repositories/{repositoryId}/versions/{versionId}/files/")]
    public class FileController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IDirectoryService _directoryService;
        private readonly IMapper _mapper;
        private readonly IVersionService _versionService;

        public FileController(IFileService fileService, IDirectoryService directoryService, IMapper mapper, IVersionService versionService)
        {
            _fileService = fileService;
            _directoryService = directoryService;
            _mapper = mapper;
            _versionService = versionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFiles(Guid userId, Guid repositoryId, Guid versionId)
        {
            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId)
                return Unauthorized();

            var repository = await _versionService.GetVersionByIdAsync(versionId);
            if (repository == null)
            {
                return BadRequest();
            }

            var files = await _fileService.GetFilesForVersionAsync(versionId);
            var filesDto = _mapper.Map<IList<FileForDisplay>>(files);

            return Ok(filesDto);
        }

        [HttpGet("{fileId}", Name = "GetFile")]
        public async Task<IActionResult> GetFile(Guid userId, Guid repositoryId, Guid versionId, Guid fileId)
        {
            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId)
                return Unauthorized();


            var file = await _fileService.GetFileByIdAsync(fileId);
            if (file == null)
            {
                return BadRequest();
            }

            var fileDto = _mapper.Map<FileForDisplay>(file);
            return Ok(fileDto);
        }

        [HttpDelete("{fileId}")]
        public async Task<IActionResult> DeleteFile(Guid userId, Guid repositoryId, Guid versionId, Guid fileId)
        {
            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId)
                return Unauthorized();

            var file = await _fileService.GetFileByIdAsync(fileId);
            if (file == null)
            {
                return BadRequest();
            }

            _fileService.DeleteFile(file);
            if (! await _fileService.SaveChangesAsync())
            {
                return StatusCode(500, "Fault while saving");
            }
            return NoContent();
        }
    }
}