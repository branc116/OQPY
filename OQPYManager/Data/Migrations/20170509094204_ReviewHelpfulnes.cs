using Microsoft.EntityFrameworkCore.Migrations;

namespace OQPYManager.Data.Migrations
{
    public partial class ReviewHelpfulnes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Helpfulness",
                table: "Reviews",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VenueId",
                table: "Reservations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_VenueId",
                table: "Reservations",
                column: "VenueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Venues_VenueId",
                table: "Reservations",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Venues_VenueId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_VenueId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Helpfulness",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "VenueId",
                table: "Reservations");
        }
    }
}