using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatBot.Data.Migrations
{
    public partial class addRoletoTableMenuRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Expanded",
                table: "MenuRoles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MenuRoles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "MenuRoles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Selected",
                table: "MenuRoles",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "MenuRoles",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expanded",
                table: "MenuRoles");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "MenuRoles");

            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "MenuRoles");

            migrationBuilder.DropColumn(
                name: "Selected",
                table: "MenuRoles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MenuRoles");
        }
    }
}
