using CommandsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandsService.Data
{
   public interface ICommandRepo
    {
        bool SaveChanges();

        //Platforms
        IEnumerable<Models.Platform> GetAllPlatforms();
        void CreatePlatform(Models.Platform platform);
        bool IsPlatformExist(int id);
        bool IsExternalPlatformExist(int externalPlatformId);

        //Commands

        IEnumerable<Models.Command> GetCommandsByPlatform(int platformId);
        Command GetCommand(int platformId, int commandId);
        void AddCommand(int platformId,Models.Command command);
        
    }
}
