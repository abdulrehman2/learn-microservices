using CommandsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _entities;

        public CommandRepo(AppDbContext entities)
        {
            _entities = entities;
        }

        #region Platform
        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _entities.Platforms.ToList();
        }

        public bool IsPlatformExist(int id)
        {
            return _entities.Platforms.Any(x => x.Id == id);
        }

        public bool IsExternalPlatformExist(int externalPlatformId)
        {
            return _entities.Platforms.Any(x => x.ExternalId == externalPlatformId);
        }

        

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException();
            }
            _entities.Platforms.Add(platform);
        }
        #endregion

        #region Command
        public void AddCommand(int platformId, Command command)
        {
            command.PlatformId = platformId; 
            _entities.Commands.Add(command);
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _entities.Commands.Where(x => x.Id == commandId && x.PlatformId == platformId).FirstOrDefault();
        }

        public IEnumerable<Command> GetCommandsByPlatform(int platformId)
        {
            return _entities.Commands.Where(x => x.PlatformId == platformId).OrderBy(x => x.Platform.Name).ToList();
        }
        #endregion

        public bool SaveChanges()
        {
            return _entities.SaveChanges() > 0;
        }
    }
}
