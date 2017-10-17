using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryApp.Data.Dto;
using RepositoryApp.Service.Services.Interfaces;

namespace RepositoryApp.API.Controllers
{
    [Route("api/Repositories")]
    public class RepositoryController : Controller
    {
        private readonly IRepositoryService _repositoryService;
        private readonly IDirectoryRepositoryService _directoryRepositoryService;
        private readonly IMapper _mapper;
        private readonly Guid _userId = Guid.Parse("0b063bd3-31f3-433c-bb94-e69d7c2e3198");

        public RepositoryController(IRepositoryService repositoryService, IDirectoryRepositoryService directoryRepositoryService, IMapper mapper)
        {
            _repositoryService = repositoryService;
            _directoryRepositoryService = directoryRepositoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetRepositories()
        {
            var repositories = await _repositoryService.GetRepositoriesForUser(_userId);
            var repositoryDto = _mapper.Map<List<RepositoryForDisplayDto>>(repositories);
            return Ok(repositoryDto);
        }
    }
}