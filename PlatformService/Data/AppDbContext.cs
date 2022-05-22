using Microsoft.EntityFrameworkCore;
using PlatformService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions contextOptions) : base(contextOptions)
        {

        }
        public DbSet<Platform> Platforms { get; set; }

    }
}
