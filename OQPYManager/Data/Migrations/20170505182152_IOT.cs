using Microsoft.EntityFrameworkCore.Migrations;

namespace OQPYManager.Data.Migrations
{
    public partial class IOT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IOTEnabled",
                table: "Resources",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OQPYed",
                table: "Resources",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecreteCode",
                table: "Resources",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IOTEnabled",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "OQPYed",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "SecreteCode",
                table: "Resources");
        }
    }
}