using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IMapper _mapper;
        private readonly IVersionService _versionService;

        public FileController(IFileService fileService, IMapper mapper, IVersionService versionService)
        {
            _fileService = fileService;
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
                return BadRequest();

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
                return BadRequest();

            var fileDto = _mapper.Map<FileForDisplay>(file);
            return Ok(fileDto);
        }

        //[HttpGet("{fileId}", Name = "DownloadFile")]
        //public async Task<IActionResult> DowloadFile(Guid fileId)
        //{
        //    throw NotImplementedException;
        //}

        [HttpDelete("{fileId}")]
        public async Task<IActionResult> DeleteFile(Guid userId, Guid repositoryId, Guid versionId, Guid fileId)
        {
            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId)
                return Unauthorized();

            var file = await _fileService.GetFileByIdAsync(fileId);
            if (file == null)
                return BadRequest();

            _fileService.DeleteFile(file);
            if (!await _fileService.SaveChangesAsync())
                return StatusCode(500, "Fault while saving");
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(Guid userId, Guid repositoryId, Guid versionId, [FromForm] IFormFile file)
        {
            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId)
                return Unauthorized();

            var version = await _versionService.GetVersionByIdAsync(versionId);
            if (version == null)
            {
                return BadRequest();
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest();
            }
            

            var path = Path.Combine(version.Path, file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileForCreate = new FileForCreation
            {
                Name = file.FileName
            };

            var fileToAdd = _mapper.Map<Data.Model.File>(fileForCreate);
            if (! await _fileService.SaveChangesAsync())
            {
                return StatusCode(500, "Fail while saving");
            }
            var fileDto = _mapper.Map<FileForDisplay>(fileToAdd);
            return Ok(fileDto);
        }


        //[HttpGet("{fileName}")]
        //public async Task<IActionResult> Download(string fileName)
        //{
        //    if (fileName == null)
        //    {
        //        return BadRequest();
        //    }

        //    var path = Path.Combine(
        //        Directory.GetCurrentDirectory(),
        //        "wwwroot", fileName);

        //    var memory = new MemoryStream();
        //    using (var stream = new FileStream(path, FileMode.Open))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;
        //    return File(memory, GetContentType(path), Path.GetFileName(path));
        //}

        //private string GetContentType(string path)
        //{
        //    var types = GetMimeTypes();
        //    var ext = Path.GetExtension(path).ToLowerInvariant();
        //    return types[ext];
        //}

        //private Dictionary<string, string> GetMimeTypes()
        //{
        //    return new Dictionary<string, string>
        //    {
        //        {".txt", "text/plain"},
        //        {".pdf", "application/pdf"},
        //        {".doc", "application/vnd.ms-word"},
        //        {".docx", "application/vnd.ms-word"},
        //        {".xls", "application/vnd.ms-excel"},
        //        {
        //            ".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
        //        {".png", "image/png"
        //        },
        //        {".jpg", "image/jpeg"},
        //        {".jpeg", "image/jpeg"},
        //        {".gif", "image/gif"},
        //        {".csv", "text/csv"}
        //    };

        //}
    }
}