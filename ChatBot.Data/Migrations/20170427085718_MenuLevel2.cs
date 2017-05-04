using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatBot.Data.Migrations
{
    public partial class MenuLevel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MenuParent",
                table: "MenuRoles",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MenuParent",
                table: "MenuRoles",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
