using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using AutoMapper;
using RepositoryApp.Data.Dto;
using RepositoryApp.Data.Model;
using RepositoryApp.Service.Helpers;
using File = RepositoryApp.Data.Model.File;
using Version = RepositoryApp.Data.Model.Version;

namespace RepositoryApp.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            var random = string.Empty;
            CreateMap<UserForCreationDto, User>()
                .ForMember(dest => dest.UniqueName,
                    opt => opt.MapFrom(
                        src => $"{CreateUsername(src.FirstName, src.LastName)}_{random.RandomString(10)}"));

            CreateMap<User, UserForDisplayDto>()
                .ForMember(dest => dest.CreatedDateTime,
                    opt => opt.MapFrom(src => src.CreationDateTime.ToString("G")))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
            CreateMap<UserForLoginDto, User>();

            CreateMap<RepositoryForCreationDto, Repository>()
                .ForMember(dest => dest.UniqueName,
                    opt => opt.MapFrom(src => $"{src.Name.Replace(' ', '_')}_{random.RandomString(10)}"));

            CreateMap<Repository, RepositoryForDisplayDto>()
                .ForMember(dest => dest.CountOfVersion,
                    opt => opt.MapFrom(
                        src => src.Versions.Count))
                .ForMember(dest => dest.CreationDateTime,
                    opt => opt.MapFrom(
                        src => src.CreationDateTime.ToString("G")));

            CreateMap<VersionForCreation, Version>()
                .ForMember(dest => dest.UniqueName,
                    opt => opt.MapFrom(src => $"{src.Name.Replace(' ', '_')}_{random.RandomString(10)}"))
                .ForMember(dest => dest.ProductionVersion,
                    opt => opt.UseValue(false));

            CreateMap<Version, VersionForDisplay>()
                .ForMember(dest => dest.CountOfFiles,
                    opt => opt.MapFrom(
                        src => src.Files.Count))
                .ForMember(dest => dest.CreationDateTime,
                    opt => opt.MapFrom(
                        src => src.CreationDateTime.ToString("G")));

            CreateMap<FileForCreation, File>();

            CreateMap<File, FileForDisplay>()
                .ForMember(dest => dest.CreationDateTime,
                    opt => opt.MapFrom(
                        src => src.CreationDateTime.ToString("G")));
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