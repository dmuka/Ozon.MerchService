using FluentMigrator;

namespace Ozon.MerchService.Migrator.Migrations;

[Migration(1)]
public class MerchPacks_Table : Migration {
    public override void Up()
    {
        Execute.Sql(@"
           CREATE TABLE IF NOT EXISTS MerchPacks(
               id BIGSERIAL PRIMARY KEY,
               name TEXT NOT NULL,
               items TEXT NOT NULL
           );");
    }

    public override void Down()
    {
        Execute.Sql(@"DROP TABLE IF EXISTS MerchPacks;");
    }
}