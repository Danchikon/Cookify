using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cookify.Infrastructure.Persistence.Migrations
{
    public partial class Quartz : Migration
    {
        private const string PathToQuartzSqlMigration = "Persistence/Migrations/SqlScripts/QuartzDbMigration.sql";
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sqlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PathToQuartzSqlMigration);
            migrationBuilder.Sql(File.ReadAllText(sqlFile));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          
        }
    }
}
