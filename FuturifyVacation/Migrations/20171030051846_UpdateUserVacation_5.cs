using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FuturifyVacation.Migrations
{
    public partial class UpdateUserVacation_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserVacations_AspNetUsers_UserId",
                table: "UserVacations");

            migrationBuilder.AddForeignKey(
                name: "FK_UserVacations_UserProfiles_UserId",
                table: "UserVacations",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserVacations_UserProfiles_UserId",
                table: "UserVacations");

            migrationBuilder.AddForeignKey(
                name: "FK_UserVacations_AspNetUsers_UserId",
                table: "UserVacations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
