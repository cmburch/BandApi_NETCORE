using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BandApi.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BandApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Use this code in prod
            //CreateHostBuilder(args).Build().Run();


            //This delete and creates a new database on start , use only in development
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetService<BandAlbumContext>();
                    context.Database.EnsureDeleted();
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occured while migration was in progress");
                }
            }

            host.Run();
            //end
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
