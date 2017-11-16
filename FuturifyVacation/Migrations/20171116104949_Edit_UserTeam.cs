using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FuturifyVacation.Migrations
{
    public partial class Edit_UserTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamDetails_AspNetUsers_ApplicationUserId",
                table: "TeamDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamDetails_UserProfiles_UserId",
                table: "TeamDetails");

            migrationBuilder.DropIndex(
                name: "IX_TeamDetails_ApplicationUserId",
                table: "TeamDetails");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "TeamDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamDetails_AspNetUsers_UserId",
                table: "TeamDetails",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamDetails_AspNetUsers_UserId",
                table: "TeamDetails");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "TeamDetails",
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

            migrationBuilder.AddForeignKey(
                name: "FK_TeamDetails_UserProfiles_UserId",
                table: "TeamDetails",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
