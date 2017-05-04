using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatBot.Data.Migrations
{
    public partial class MenuLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MenuOrder",
                table: "MenuRoles",
                newName: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "MenuParent",
                table: "MenuRoles",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MenuLevel",
                table: "MenuRoles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuLevel",
                table: "MenuRoles");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "MenuRoles",
                newName: "MenuOrder");

            migrationBuilder.AlterColumn<string>(
                name: "MenuParent",
                table: "MenuRoles",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
