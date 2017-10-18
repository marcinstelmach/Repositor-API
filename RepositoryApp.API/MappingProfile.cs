using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RepositoryApp.Data.Dto;
using RepositoryApp.Data.Model;
using StackExchange.Redis;

namespace RepositoryApp.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Repository, RepositoryForDisplayDto>();
            CreateMap<UserForCreationDto, User>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => CreateUsername(src.FirstName, src.LastName)));
            CreateMap<User, UserForDisplayDto>()
                .ForMember(dest => dest.CreatedDateTime,
                    opt => opt.MapFrom(src => src.CreationDateTime.ToShortDateString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
            CreateMap<UserForLoginDto, User>();

            CreateMap<RepositoryForCreationDto, Repository>()
                .ForMember(dest => dest.CreationDateTime,
                    opt => opt.UseValue(DateTime.Now))
                .ForMember(dest => dest.ModifyDateTime,
                    opt => opt.UseValue(DateTime.Now));
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
