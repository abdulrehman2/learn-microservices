using PlatformService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.Data
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly Data.AppDbContext _dbContext;

        public PlatformRepository(Data.AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException();
            }
            _dbContext.Platforms.Add(platform);
        }

        public Platform GetPlatformById(int id)
        {
            return _dbContext.Platforms.Where(i => i.Id == id).FirstOrDefault();
        }

        public IEnumerable<Platform> GetPlatforms()
        {
            return _dbContext.Platforms.ToList();
        }

        public bool SaveChanges()
        {
            return _dbContext.SaveChanges() > 1;
        }
    }
}
