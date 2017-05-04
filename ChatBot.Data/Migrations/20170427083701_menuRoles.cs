using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatBot.Data.Migrations
{
    public partial class menuRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuRoles_ApplicationRoles_RoleId",
                table: "MenuRoles");

            migrationBuilder.DropIndex(
                name: "IX_MenuRoles_RoleId",
                table: "MenuRoles");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "MenuRoles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "MenuRoles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuRoles_RoleId",
                table: "MenuRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuRoles_ApplicationRoles_RoleId",
                table: "MenuRoles",
                column: "RoleId",
                principalTable: "ApplicationRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
