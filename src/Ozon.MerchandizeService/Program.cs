using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Ozon.MerchandizeService;


CreateHostBuilder(args).Build().Run();
return;

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(
            webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });