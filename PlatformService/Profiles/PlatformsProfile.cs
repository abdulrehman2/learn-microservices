using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.Profiles
{
    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
            CreateMap<Models.Platform, Dtos.PlatformReadDto>();
            CreateMap<Dtos.PlatformCreateDto, Models.Platform>();
            CreateMap<Dtos.PlatformReadDto, Dtos.PlatformPublishDto>();
            CreateMap<Models.Platform, GrpcPlatformModel>().
                ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.Id)).
                ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)).
                ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher));
        }
    }
}
