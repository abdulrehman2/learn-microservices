using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly Data.ICommandRepo _commandRepo;
        private readonly IMapper _mapper;
        public CommandsController(Data.ICommandRepo commandRepo, IMapper mapper)
        {
            _commandRepo = commandRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Dtos.CommandToRead>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");
            if (!_commandRepo.IsPlatformExist(platformId))
            {
                return NotFound();
            }
            var result = _commandRepo.GetCommandsByPlatform(platformId);
            return Ok(_mapper.Map<IEnumerable<Dtos.CommandToRead>>(result));

        }


        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<Dtos.CommandToRead> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId} / {commandId}");

            if (!_commandRepo.IsPlatformExist(platformId))
            {
                return NotFound();
            }

            var command = _commandRepo.GetCommand(platformId, commandId);

            if (command == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Dtos.CommandToRead>(command));

        }


        [HttpPost]
        public ActionResult<Dtos.CommandToRead> CreateCommandForPlatform(int platformId, Dtos.CommandToCreate command)
        {
            Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");

            if (!_commandRepo.IsPlatformExist(platformId))
            {
                return NotFound();
            }

            var model = _mapper.Map<Models.Command>(command);
            _commandRepo.AddCommand(platformId, model);
            var isSaved = _commandRepo.SaveChanges();

            var commandReadDto = _mapper.Map<Dtos.CommandToRead>(model);


            return CreatedAtRoute(nameof(GetCommandForPlatform), new { platformId = platformId, commandId = commandReadDto.Id }, commandReadDto);

        }

    }
}
