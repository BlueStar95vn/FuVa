using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FuturifyVacation.Migrations
{
    public partial class EditUserTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "profileUserId",
                table: "UserTeams",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_profileUserId",
                table: "UserTeams",
                column: "profileUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeams_UserProfiles_profileUserId",
                table: "UserTeams",
                column: "profileUserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTeams_UserProfiles_profileUserId",
                table: "UserTeams");

            migrationBuilder.DropIndex(
                name: "IX_UserTeams_profileUserId",
                table: "UserTeams");

            migrationBuilder.DropColumn(
                name: "profileUserId",
                table: "UserTeams");
        }
    }
}
