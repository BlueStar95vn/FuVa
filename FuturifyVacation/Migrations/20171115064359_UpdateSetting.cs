using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FuturifyVacation.Migrations
{
    public partial class UpdateSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndHour",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "StartHour",
                table: "Settings");

            migrationBuilder.AddColumn<int>(
                name: "EndAm",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EndPm",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HoursADay",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartAm",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartPm",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndAm",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "EndPm",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "HoursADay",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "StartAm",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "StartPm",
                table: "Settings");

            migrationBuilder.AddColumn<int>(
                name: "EndHour",
                table: "Settings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartHour",
                table: "Settings",
                nullable: false,
                defaultValue: 0);
        }
    }
}
