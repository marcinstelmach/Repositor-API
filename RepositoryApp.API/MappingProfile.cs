using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RepositoryApp.Data.Dto;
using RepositoryApp.Data.Model;

namespace RepositoryApp.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Repository, RepositoryForDisplayDto>();
        }
    }
}
