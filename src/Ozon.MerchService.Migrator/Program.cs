using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var connectionString = configuration
    .GetSection("DBConnectionOptions:ConnectionString")
    .Value;

var services = new ServiceCollection()
    .AddFluentMigratorCore()
    .ConfigureRunner(
        runner =>
            runner.AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(Program).Assembly)
                .For.Migrations())
    .AddLogging(loggingBuilder => loggingBuilder.AddFluentMigratorConsole());

var serviceProvider = services.BuildServiceProvider();

using (serviceProvider.CreateScope())
{
    var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}             