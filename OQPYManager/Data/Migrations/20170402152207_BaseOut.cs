using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace OQPYManager.Data.Migrations
{
    public partial class BaseOut: Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "BaseVenueTag");

            migrationBuilder.DropTable(
                name: "BaseWorkTime");

            migrationBuilder.RenameColumn(
                name: "BaseOwnerId",
                table: "AspNetUserRoles",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "BaseEmployeeId",
                table: "AspNetUserRoles",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_BaseOwnerId",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_BaseEmployeeId",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "BaseOwnerId",
                table: "AspNetUserLogins",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "BaseEmployeeId",
                table: "AspNetUserLogins",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_BaseOwnerId",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_BaseEmployeeId",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "BaseOwnerId",
                table: "AspNetUserClaims",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "BaseEmployeeId",
                table: "AspNetUserClaims",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_BaseOwnerId",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_BaseEmployeeId",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "BaseVenueId",
                table: "Tags",
                newName: "VenueId");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_BaseVenueId",
                table: "Tags",
                newName: "IX_Tags_VenueId");

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
                name: "IX_VenueTag_TagId",
                table: "VenueTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTime_WorkHoursId",
                table: "WorkTime",
                column: "WorkHoursId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_Employees_EmployeeId",
                table: "AspNetUserClaims",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_Owners_OwnerId",
                table: "AspNetUserClaims",
                column: "OwnerId",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_Employees_EmployeeId",
                table: "AspNetUserLogins",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_Owners_OwnerId",
                table: "AspNetUserLogins",
                column: "OwnerId",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_Employees_EmployeeId",
                table: "AspNetUserRoles",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_Owners_OwnerId",
                table: "AspNetUserRoles",
                column: "OwnerId",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Venues_VenueId",
                table: "Tags",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_Employees_EmployeeId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_Owners_OwnerId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_Employees_EmployeeId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_Owners_OwnerId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_Employees_EmployeeId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_Owners_OwnerId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Venues_VenueId",
                table: "Tags");

            migrationBuilder.DropTable(
                name: "VenueTag");

            migrationBuilder.DropTable(
                name: "WorkTime");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "AspNetUserRoles",
                newName: "BaseOwnerId");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "AspNetUserRoles",
                newName: "BaseEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_OwnerId",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_BaseOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_EmployeeId",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_BaseEmployeeId");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "AspNetUserLogins",
                newName: "BaseOwnerId");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "AspNetUserLogins",
                newName: "BaseEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_OwnerId",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_BaseOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_EmployeeId",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_BaseEmployeeId");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "AspNetUserClaims",
                newName: "BaseOwnerId");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "AspNetUserClaims",
                newName: "BaseEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_OwnerId",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_BaseOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_EmployeeId",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_BaseEmployeeId");

            migrationBuilder.RenameColumn(
                name: "VenueId",
                table: "Tags",
                newName: "BaseVenueId");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_VenueId",
                table: "Tags",
                newName: "IX_Tags_BaseVenueId");

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
        }
    }
}