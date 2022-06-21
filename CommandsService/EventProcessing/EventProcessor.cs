using AutoMapper;
using CommandsService.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CommandsService.EventProcessing
{
    public enum EventType
    {
        PlatformPublished,
        Undetermined
    }

    public class EventProcessor : IEventProcessor
    {
        private IServiceScopeFactory _serviceScopeFactory;
        private IMapper _mapper;

        public EventProcessor(IServiceScopeFactory serviceScopeFactory, AutoMapper.IMapper mapper)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;

        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    addPlatform(message);
                    break;


                case EventType.Undetermined:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<Dtos.GenericEventDto>(notificationMessage);

            switch (eventType.Event)
            {

                case "Platform_Published":
                    Console.WriteLine("Platform published event detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("Could not determine Event type");
                    return EventType.Undetermined;
            }
        }

        private void addPlatform(string platformPublishMessage)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                var publishPlatformDto = JsonSerializer.Deserialize<Dtos.PlatformPublishedDto>(platformPublishMessage);

                try
                {
                    var plat = _mapper.Map<Models.Platform>(publishPlatformDto);
                    if (!repo.IsExternalPlatformExist(plat.ExternalId))
                    {
                        repo.CreatePlatform(plat);
                        repo.SaveChanges();
                        Console.WriteLine($"--> Platform added....");
                    }
                    else
                    {
                        Console.WriteLine($"--> Platform already exist....");
                    }

                }
                catch (Exception e)
                {

                    Console.WriteLine($"Could not add Platform to DB {e.Message}");
                }
            }
        }

    }
}
