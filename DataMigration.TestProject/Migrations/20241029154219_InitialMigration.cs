using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataMigration.TestProject.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sales",
                columns: table => new
                {
                    sale_ıd = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_ıd = table.Column<Guid>(type: "uuid", nullable: false),
                    product_name = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    sale_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sales", x => new { x.sale_ıd, x.tenant_ıd });
                });

            migrationBuilder.Sql("SELECT create_distributed_table('sales', 'tenant_ıd');");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sales");
        }
    }
}
