using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OQPYManager.Data.Migrations
{
    public partial class ToBaseWeGo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Venues_VenueId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Venues_VenueId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Venues_AspNetUsers_OwnerId",
                table: "Venues");

            migrationBuilder.DropTable(
                name: "VenueTag");

            migrationBuilder.DropTable(
                name: "WorkTime");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_VenueId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VenueId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "VenueId",
                table: "Tags",
                newName: "BaseVenueId");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_VenueId",
                table: "Tags",
                newName: "IX_Tags_BaseVenueId");

            migrationBuilder.AddColumn<string>(
                name: "BaseEmployeeId",
                table: "AspNetUserRoles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BaseOwnerId",
                table: "AspNetUserRoles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BaseEmployeeId",
                table: "AspNetUserLogins",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BaseOwnerId",
                table: "AspNetUserLogins",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BaseEmployeeId",
                table: "AspNetUserClaims",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BaseOwnerId",
                table: "AspNetUserClaims",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Venues",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    VenueId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaseVenueTag",
                columns: table => new
                {
                    VenueId = table.Column<string>(nullable: false),
                    TagId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseVenueTag", x => new { x.VenueId, x.TagId });
                    table.ForeignKey(
                        name: "FK_BaseVenueTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseVenueTag_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaseWorkTime",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    WorkHoursId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseWorkTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseWorkTime_VenueWorkHours_WorkHoursId",
                        column: x => x.WorkHoursId,
                        principalTable: "VenueWorkHours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_BaseEmployeeId",
                table: "AspNetUserRoles",
                column: "BaseEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_BaseOwnerId",
                table: "AspNetUserRoles",
                column: "BaseOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_BaseEmployeeId",
                table: "AspNetUserLogins",
                column: "BaseEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_BaseOwnerId",
                table: "AspNetUserLogins",
                column: "BaseOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_BaseEmployeeId",
                table: "AspNetUserClaims",
                column: "BaseEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_BaseOwnerId",
                table: "AspNetUserClaims",
                column: "BaseOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_VenueId",
                table: "Employees",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseVenueTag_TagId",
                table: "BaseVenueTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseWorkTime_WorkHoursId",
                table: "BaseWorkTime",
                column: "WorkHoursId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_Employees_BaseEmployeeId",
                table: "AspNetUserClaims",
                column: "BaseEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_Owners_BaseOwnerId",
                table: "AspNetUserClaims",
                column: "BaseOwnerId",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_Employees_BaseEmployeeId",
                table: "AspNetUserLogins",
                column: "BaseEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_Owners_BaseOwnerId",
                table: "AspNetUserLogins",
                column: "BaseOwnerId",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_Employees_BaseEmployeeId",
                table: "AspNetUserRoles",
                column: "BaseEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_Owners_BaseOwnerId",
                table: "AspNetUserRoles",
                column: "BaseOwnerId",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Venues_BaseVenueId",
                table: "Tags",
                column: "BaseVenueId",
                principalTable: "Venues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Venues_Owners_OwnerId",
                table: "Venues",
                column: "OwnerId",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_Employees_BaseEmployeeId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_Owners_BaseOwnerId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_Employees_BaseEmployeeId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_Owners_BaseOwnerId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_Employees_BaseEmployeeId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_Owners_BaseOwnerId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Venues_BaseVenueId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Venues_Owners_OwnerId",
                table: "Venues");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Owners");

            migrationBuilder.DropTable(
                name: "BaseVenueTag");

            migrationBuilder.DropTable(
                name: "BaseWorkTime");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_BaseEmployeeId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_BaseOwnerId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserLogins_BaseEmployeeId",
                table: "AspNetUserLogins");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserLogins_BaseOwnerId",
                table: "AspNetUserLogins");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserClaims_BaseEmployeeId",
                table: "AspNetUserClaims");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserClaims_BaseOwnerId",
                table: "AspNetUserClaims");

            migrationBuilder.DropColumn(
                name: "BaseEmployeeId",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "BaseOwnerId",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "BaseEmployeeId",
                table: "AspNetUserLogins");

            migrationBuilder.DropColumn(
                name: "BaseOwnerId",
                table: "AspNetUserLogins");

            migrationBuilder.DropColumn(
                name: "BaseEmployeeId",
                table: "AspNetUserClaims");

            migrationBuilder.DropColumn(
                name: "BaseOwnerId",
                table: "AspNetUserClaims");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Venues");

            migrationBuilder.RenameColumn(
                name: "BaseVenueId",
                table: "Tags",
                newName: "VenueId");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_BaseVenueId",
                table: "Tags",
                newName: "IX_Tags_VenueId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VenueId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VenueTag",
                columns: table => new
                {
                    VenueId = table.Column<string>(nullable: false),
                    TagId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueTag", x => new { x.VenueId, x.TagId });
                    table.ForeignKey(
                        name: "FK_VenueTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VenueTag_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkTime",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    WorkHoursId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkTime_VenueWorkHours_WorkHoursId",
                        column: x => x.WorkHoursId,
                        principalTable: "VenueWorkHours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_VenueId",
                table: "AspNetUsers",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_VenueTag_TagId",
                table: "VenueTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTime_WorkHoursId",
                table: "WorkTime",
                column: "WorkHoursId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Venues_VenueId",
                table: "AspNetUsers",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Venues_VenueId",
                table: "Tags",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Venues_AspNetUsers_OwnerId",
                table: "Venues",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
