using Ozon.MerchandizeService;
using Ozon.MerchandizeService.Configuration.Extensions;


CreateHostBuilder(args).Build().Run();
return;

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(
            webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
        .AddInfrastructure();