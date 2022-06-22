using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandsService.Data
{
    public static class PrepData
    {
        public static void PrePopulation (IApplicationBuilder applicationBuilder)
        {
            using(var scope= applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = scope.ServiceProvider.GetService<IPlatformDataClient>();
                var platforms = grpcClient.ReturnAllPlatforms();
                SeedData(scope.ServiceProvider.GetService<ICommandRepo>(), platforms);
            }
        }

        private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("--> Seeding new platforms");

            foreach (var plat in platforms)
            {
                if (!repo.IsExternalPlatformExist(plat.ExternalId))
                {
                    repo.CreatePlatform(plat);
                }
            }
            repo.SaveChanges();
        }

    }
}
