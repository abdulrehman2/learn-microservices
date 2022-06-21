using AutoMapper;
using CommandsService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepo _commandRepo;
        private readonly IMapper _mapper;
        public PlatformsController(ICommandRepo commandRepo, IMapper mapper)
        {
            _commandRepo = commandRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult GetPlatforms()
        {
            var platforms = _commandRepo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<Dtos.PlatformToRead>>(platforms));
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("==> Inbound Post # Command Service");
            return Ok("Inbound Test of Platform Controller");
        }

    }
}
