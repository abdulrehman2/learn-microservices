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
        }
    }
}
