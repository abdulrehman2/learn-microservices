using CommandsService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandsService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Models.Platform> Platforms { get; set; }
        public DbSet<Models.Command> Commands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Platform>().HasMany(p => p.Commands).WithOne(x => x.Platform!).HasForeignKey(p => p.PlatformId);
            modelBuilder.Entity<Command>().HasOne(p => p.Platform).WithMany(x => x.Commands).HasForeignKey(p => p.PlatformId);

        }
    }
}
