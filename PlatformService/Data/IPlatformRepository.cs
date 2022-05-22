using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.Data
{
    public interface IPlatformRepository
    {
        void CreatePlatform(Models.Platform platform);
        IEnumerable<Models.Platform> GetPlatforms();
        Models.Platform GetPlatformById(int id);

        bool SaveChanges();

    }
}
