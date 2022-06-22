using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PlatformService.SyncDataServices.Grpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            if (_env.IsProduction())
            {
                Console.WriteLine("Using SQL Server");
                services.AddDbContext<Data.AppDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("PlatformsConn")));
            }
            else
            {
                Console.WriteLine("Using IN Memory Database");
                services.AddDbContext<Data.AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
            }

            services.AddScoped<Data.IPlatformRepository, Data.PlatformRepository>();
            services.AddSingleton<AsyncDataServices.IMessageBusClient, AsyncDataServices.MessageBusClient>();
            services.AddGrpc();
            services.AddControllers();
            services.AddHttpClient<SyncDataServices.Http.ICommandDataClient, SyncDataServices.Http.HttpCommandDataClient>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlatformService", Version = "v1" });
            });

            Console.WriteLine($"Commnad Service URL: {Configuration["CommandService"]}");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformService v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<GrpcPlatformService>();

                endpoints.MapGet("/protos/platforms.proto", async context =>
                 {
                     await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
                 });
            });

            Data.PreDB.InitializeDB(app, env.IsProduction());
        }
    }
}
