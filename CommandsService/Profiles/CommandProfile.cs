using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandsService.Profiles
{
    public class CommandProfile:Profile
    {
        public CommandProfile()
        {
            CreateMap<Platform, PlatformToRead>();
            CreateMap<CommandToCreate, Command>();
            CreateMap<Command, CommandToRead>();
            CreateMap<PlatformPublishedDto, Platform>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
