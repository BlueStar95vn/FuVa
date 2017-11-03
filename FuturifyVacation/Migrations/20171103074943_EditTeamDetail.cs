using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FuturifyVacation.Migrations
{
    public partial class EditTeamDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamDetails_UserTeams_TeamId",
                table: "TeamDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamDetails_UserProfiles_UserId",
                table: "TeamDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeams_UserProfiles_profileUserId",
                table: "UserTeams");

            migrationBuilder.DropIndex(
                name: "IX_UserTeams_profileUserId",
                table: "UserTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamDetails",
                table: "TeamDetails");

            migrationBuilder.DropIndex(
                name: "IX_TeamDetails_UserId",
                table: "TeamDetails");

            migrationBuilder.DropColumn(
                name: "profileUserId",
                table: "UserTeams");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TeamDetails");

            migrationBuilder.AlterColumn<string>(
                name: "TeamLeadId",
                table: "UserTeams",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "TeamDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Team_Id",
                table: "TeamDetails",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "ProfileUserId",
                table: "TeamDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "User_Id",
                table: "TeamDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamDetails",
                table: "TeamDetails",
                column: "Team_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_TeamLeadId",
                table: "UserTeams",
                column: "TeamLeadId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamDetails_ProfileUserId",
                table: "TeamDetails",
                column: "ProfileUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamDetails_TeamId",
                table: "TeamDetails",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamDetails_UserProfiles_ProfileUserId",
                table: "TeamDetails",
                column: "ProfileUserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamDetails_UserTeams_TeamId",
                table: "TeamDetails",
                column: "TeamId",
                principalTable: "UserTeams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeams_UserProfiles_TeamLeadId",
                table: "UserTeams",
                column: "TeamLeadId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamDetails_UserProfiles_ProfileUserId",
                table: "TeamDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamDetails_UserTeams_TeamId",
                table: "TeamDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeams_UserProfiles_TeamLeadId",
                table: "UserTeams");

            migrationBuilder.DropIndex(
                name: "IX_UserTeams_TeamLeadId",
                table: "UserTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamDetails",
                table: "TeamDetails");

            migrationBuilder.DropIndex(
                name: "IX_TeamDetails_ProfileUserId",
                table: "TeamDetails");

            migrationBuilder.DropIndex(
                name: "IX_TeamDetails_TeamId",
                table: "TeamDetails");

            migrationBuilder.DropColumn(
                name: "Team_Id",
                table: "TeamDetails");

            migrationBuilder.DropColumn(
                name: "ProfileUserId",
                table: "TeamDetails");

            migrationBuilder.DropColumn(
                name: "User_Id",
                table: "TeamDetails");

            migrationBuilder.AlterColumn<string>(
                name: "TeamLeadId",
                table: "UserTeams",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "profileUserId",
                table: "UserTeams",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "TeamDetails",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TeamDetails",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamDetails",
                table: "TeamDetails",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_profileUserId",
                table: "UserTeams",
                column: "profileUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamDetails_UserId",
                table: "TeamDetails",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamDetails_UserTeams_TeamId",
                table: "TeamDetails",
                column: "TeamId",
                principalTable: "UserTeams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamDetails_UserProfiles_UserId",
                table: "TeamDetails",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeams_UserProfiles_profileUserId",
                table: "UserTeams",
                column: "profileUserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
