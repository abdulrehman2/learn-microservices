﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.Data
{
    public static class PreDB
    {
        public static void InitializeDB(IApplicationBuilder app,bool isProd)
        {
            using(var scope = app.ApplicationServices.CreateScope())
            {
                SeedData(scope.ServiceProvider.GetService<AppDbContext>(), isProd);

            }
        }
        private static bool SeedData(AppDbContext dbContext, bool isProd)
        {
            if (isProd)
            {
                try
                {
                    Console.WriteLine("--> Attempting to perform migration");
                    dbContext.Database.Migrate();
                    
                }
                catch(Exception e)
                {
                    Console.WriteLine("--> Unable to perform migration");
                }
            }


            if (!dbContext.Platforms.Any())
            {
                dbContext.Platforms.Add(new Models.Platform { Name = "Dot Net", Cost = "Free", Publisher = "Microsoft" });
                dbContext.Platforms.Add(new Models.Platform { Name = "SQL Server", Cost = "Free", Publisher = "Microsoft" });
                dbContext.Platforms.Add(new Models.Platform { Name = "Kubernetes", Cost = "Free", Publisher = "Cloud Native Computing Foundation" });
                dbContext.SaveChanges();
                return true;
            }

            else
            {
                Console.WriteLine("Data populated already");
                return false;
            }
        }
    }
}
