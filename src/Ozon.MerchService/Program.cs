using Ozon.MerchService;
using Ozon.MerchService.Configuration.Extensions;


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