﻿using System;
using System.Globalization;
using System.Linq;
using System.Text;
using AutoMapper;
using RepositoryApp.Data.Dto;
using RepositoryApp.Data.Model;
using RepositoryApp.Service.Helpers;

namespace RepositoryApp.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            var random = string.Empty;
            CreateMap<Repository, RepositoryForDisplayDto>()
                .ForMember(dest => dest.CountOfVersion,
                    opt => opt.MapFrom(
                        src => src.Versions.Count));

            CreateMap<UserForCreationDto, User>()
                .ForMember(dest => dest.UniqueName,
                    opt => opt.MapFrom(
                        src => $"{CreateUsername(src.FirstName, src.LastName)}_{random.RandomString(10)}"));


            CreateMap<User, UserForDisplayDto>()
                .ForMember(dest => dest.CreatedDateTime,
                    opt => opt.MapFrom(src => src.CreationDateTime.ToShortDateString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
            CreateMap<UserForLoginDto, User>();

            CreateMap<RepositoryForCreationDto, Repository>()
                .ForMember(dest => dest.CreationDateTime,
                    opt => opt.UseValue(DateTime.Now))
                    .ForMember(dest => dest.UniqueName,
                    opt => opt.MapFrom(src => $"{src.Name.Replace(' ', '_')}_{random.RandomString(10)}"));

            CreateMap<VersionForCreation, Data.Model.Version>()
                .ForMember(dest => dest.CreationDateTime,
                    opt => opt.UseValue(DateTime.Now))
                .ForMember(dest => dest.UniqueName,
                    opt => opt.MapFrom(src => $"{src.Name.Replace(' ', '_')}_{random.RandomString(10)}"));

            CreateMap<Data.Model.Version, VersionForDisplay>()
                .ForMember(dest => dest.CountOfFiles,
                    opt => opt.MapFrom(
                        src => src.Files.Count));

            CreateMap<FileForCreation, File>()
                .ForMember(dest => dest.CreationDateTime,
                    opt => opt.UseValue(DateTime.Now));
            CreateMap<File, FileForDisplay>();
            CreateMap<FileForCreation, File>();
        }

        private static string CreateUsername(string firstName, string lastName)
        {
            var userName =
                $"{firstName.ToLower()}{lastName.First().ToString().ToUpper()}{lastName.Substring(1).ToLower()}";
            var normalizedString = userName.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c == 'ł' ? 'l' : c);
            }
            var result = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

            return result;
        }
    }
}