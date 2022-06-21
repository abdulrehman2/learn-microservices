using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Dtos;
using PlatformService.SyncDataServices.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly Data.IPlatformRepository _platformRepository;
        private readonly IMapper _mappper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;
        public PlatformsController(Data.IPlatformRepository platformRepository, IMapper mappper, ICommandDataClient commandDataClient, IMessageBusClient messageBusClient)
        {
            _platformRepository = platformRepository;
            _mappper = mappper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {

            var result = _platformRepository.GetPlatforms();
            return Ok(_mappper.Map<IEnumerable<PlatformReadDto>>(result));

        }

        [HttpGet(template:"{id}",Name ="GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var result = _platformRepository.GetPlatformById(id);
            return Ok(_mappper.Map<PlatformReadDto>(result));
        }


        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(Dtos.PlatformCreateDto platform)
        {
            var model = _mappper.Map<Models.Platform>(platform);
            _platformRepository.CreatePlatform(model);
            _platformRepository.SaveChanges();
            var platformToRead = _mappper.Map<PlatformReadDto>(model);

            //Send Message Synchronously
            try
            {
               await _commandDataClient.SendPlatformToCommand(platformToRead);
            }
            catch(Exception e)
            {
                Console.WriteLine($"--> Could not send synchronously:{e.Message}");
            }

            //Send Message Async
            try
            {
                var publishDto = _mappper.Map<Dtos.PlatformPublishDto>(platformToRead);
                publishDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(publishDto);
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Could not send asynchronously:{e.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { id = platformToRead.Id }, platformToRead);
        }
    }
}
