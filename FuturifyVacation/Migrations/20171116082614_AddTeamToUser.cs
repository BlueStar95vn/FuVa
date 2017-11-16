using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FuturifyVacation.Migrations
{
    public partial class AddTeamToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserProfiles");

            migrationBuilder.AddColumn<int>(
                name: "Hours",
                table: "UserVacations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "TeamDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamDetails_ApplicationUserId",
                table: "TeamDetails",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamDetails_AspNetUsers_ApplicationUserId",
                table: "TeamDetails",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamDetails_AspNetUsers_ApplicationUserId",
                table: "TeamDetails");

            migrationBuilder.DropIndex(
                name: "IX_TeamDetails_ApplicationUserId",
                table: "TeamDetails");

            migrationBuilder.DropColumn(
                name: "Hours",
                table: "UserVacations");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "TeamDetails");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "UserProfiles",
                nullable: true);
        }
    }
}
