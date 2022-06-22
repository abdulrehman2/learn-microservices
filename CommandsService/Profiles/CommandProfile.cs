using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using PlatformService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandsService.Profiles
{
    public class CommandProfile : Profile
    {
        public CommandProfile()
        {
            CreateMap<Platform, PlatformToRead>();
            CreateMap<CommandToCreate, Command>();
            CreateMap<Command, CommandToRead>();
            CreateMap<PlatformPublishedDto, Platform>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<GrpcPlatformModel, Models.Platform>().
            ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.PlatformId)).
            ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)).
            ForMember(dest => dest.Commands, opt => opt.Ignore()); 


        }
    }
}
