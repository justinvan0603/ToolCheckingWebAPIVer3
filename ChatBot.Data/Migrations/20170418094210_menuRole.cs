using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ChatBot.Data.Migrations
{
    public partial class menuRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BOT_DOMAINs");

            migrationBuilder.CreateTable(
                name: "MenuRoles",
                columns: table => new
                {
                    MenuId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthStatus = table.Column<string>(nullable: true),
                    CheckerId = table.Column<string>(nullable: true),
                    DateApprove = table.Column<DateTime>(nullable: true),
                    Isapprove = table.Column<string>(nullable: true),
                    IsapproveFunc = table.Column<string>(nullable: true),
                    MakerId = table.Column<string>(nullable: true),
                    MenuLink = table.Column<string>(nullable: true),
                    MenuName = table.Column<string>(nullable: true),
                    MenuNameEl = table.Column<string>(nullable: true),
                    MenuOrder = table.Column<int>(nullable: true),
                    MenuParent = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuRoles", x => x.MenuId);
                    table.ForeignKey(
                        name: "FK_MenuRoles_ApplicationRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ApplicationRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuRoles_RoleId",
                table: "MenuRoles",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuRoles");

            migrationBuilder.CreateTable(
                name: "BOT_DOMAINs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DOMAIN = table.Column<string>(nullable: true),
                    DOMAIN_ID = table.Column<int>(nullable: false),
                    RECORD_STATUS = table.Column<int>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOT_DOMAINs", x => x.Id);
                });
        }
    }
}
