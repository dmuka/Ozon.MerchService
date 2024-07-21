using FluentMigrator;

namespace Ozon.MerchService.Migrator.Migrations;

[Migration(1)]
public class Initial_Create_tables : Migration {
    public override void Up()
    {
        Execute.Sql(@"
           CREATE TABLE IF NOT EXISTS merchpacks(
               id bigserial primary key,
               name varchar(20) not null,
               items bigserial[] not null
           );");
        
        Execute.Sql(@"
           CREATE TABLE IF NOT EXISTS employees(
               id bigserial primary key,
               full_name varchar(100) not null,
               email varchar(50) not null,
               clothing_size int not null
           );");
        
        Execute.Sql(@"
           CREATE TABLE IF NOT EXISTS merchpack_requests(
               id bigserial primary key,
               merchpack_id int not null,
               employee_id bigserial  not null,
               hr_email varchar(50) not null,
               request_type 
               requested_at timestamptz default (now() at time zone 'utc'),
               issued  timestamptz default null,
               status int not null
           );");
        
        Execute.Sql(@"
           CREATE TABLE IF NOT EXISTS request_statuses(
               id int primary key,
               name varchar(20) not null
           );");
        
        Execute.Sql(@"
           CREATE TABLE IF NOT EXISTS clothing_sizes(
               id int primary key,
               name varchar(3) not null
           );");
    }

    public override void Down()
    {
        Execute.Sql(@"DROP TABLE IF EXISTS merchpacks;");
        Execute.Sql(@"DROP TABLE IF EXISTS employees;");
        Execute.Sql(@"DROP TABLE IF EXISTS merchpack_requests;");
        Execute.Sql(@"DROP TABLE IF EXISTS request_statuses;");
        Execute.Sql(@"DROP TABLE IF EXISTS clothing_sizes;");
    }
}