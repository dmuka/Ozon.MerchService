using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using Npgsql;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var connectionString = configuration
    .GetSection("DBConnectionOptions:ConnectionString")
    .Value;

using (var serviceProvider = CreateServices())
using (var scope = serviceProvider.CreateScope())
{
    UpdateDatabase(scope.ServiceProvider, connectionString, args);
}

return;

ServiceProvider CreateServices()
{
    return new ServiceCollection()
        .AddFluentMigratorCore()
        .ConfigureRunner(rb => rb
            .AddPostgres()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(typeof(Program).Assembly).For.Migrations())
        .AddLogging(lb => lb.AddFluentMigratorConsole())
        .BuildServiceProvider(false);
}

static void UpdateDatabase(
    IServiceProvider serviceProvider, 
    string connectionString, 
    string[] args)
{
    var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
    
    if (args.Contains("--dryrun"))
    {
        runner.ListMigrations();
    }
    else
    {
        runner.MigrateUp();
    }

    using var connection = new NpgsqlConnection(connectionString);
    connection.Open();
    connection.ReloadTypes();
    runner.MigrateUp();
}    