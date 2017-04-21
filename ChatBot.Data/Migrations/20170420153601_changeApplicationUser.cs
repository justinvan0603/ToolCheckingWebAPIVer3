using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatBot.Data.Migrations
{
    public partial class changeApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthStatus",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "CheckerId",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "EditorId",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "MakerId",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "RecordStatus",
                table: "ApplicationUser");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "ApplicationUser",
                newName: "PHONE");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "ApplicationUser",
                newName: "PASSWORD");

            migrationBuilder.RenameColumn(
                name: "Fullname",
                table: "ApplicationUser",
                newName: "FULLNAME");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ApplicationUser",
                newName: "DESCRIPTION");

            migrationBuilder.RenameColumn(
                name: "Apptoken",
                table: "ApplicationUser",
                newName: "APPTOKEN");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "ApplicationUser",
                newName: "PARENT_ID");

            migrationBuilder.RenameColumn(
                name: "EditDt",
                table: "ApplicationUser",
                newName: "EDIT_DT");

            migrationBuilder.RenameColumn(
                name: "CreateDt",
                table: "ApplicationUser",
                newName: "CREATE_DT");

            migrationBuilder.RenameColumn(
                name: "ApproveDt",
                table: "ApplicationUser",
                newName: "APPROVE_DT");

            migrationBuilder.AlterColumn<string>(
                name: "FULLNAME",
                table: "ApplicationUser",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DESCRIPTION",
                table: "ApplicationUser",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "APPTOKEN",
                table: "ApplicationUser",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AUTH_STATUS",
                table: "ApplicationUser",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CHECKER_ID",
                table: "ApplicationUser",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EDITOR_ID",
                table: "ApplicationUser",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MAKER_ID",
                table: "ApplicationUser",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RECORD_STATUS",
                table: "ApplicationUser",
                maxLength: 1,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AUTH_STATUS",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "CHECKER_ID",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "EDITOR_ID",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "MAKER_ID",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "RECORD_STATUS",
                table: "ApplicationUser");

            migrationBuilder.RenameColumn(
                name: "PHONE",
                table: "ApplicationUser",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "PASSWORD",
                table: "ApplicationUser",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "FULLNAME",
                table: "ApplicationUser",
                newName: "Fullname");

            migrationBuilder.RenameColumn(
                name: "DESCRIPTION",
                table: "ApplicationUser",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "APPTOKEN",
                table: "ApplicationUser",
                newName: "Apptoken");

            migrationBuilder.RenameColumn(
                name: "PARENT_ID",
                table: "ApplicationUser",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "EDIT_DT",
                table: "ApplicationUser",
                newName: "EditDt");

            migrationBuilder.RenameColumn(
                name: "CREATE_DT",
                table: "ApplicationUser",
                newName: "CreateDt");

            migrationBuilder.RenameColumn(
                name: "APPROVE_DT",
                table: "ApplicationUser",
                newName: "ApproveDt");

            migrationBuilder.AlterColumn<string>(
                name: "Fullname",
                table: "ApplicationUser",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ApplicationUser",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Apptoken",
                table: "ApplicationUser",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthStatus",
                table: "ApplicationUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckerId",
                table: "ApplicationUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditorId",
                table: "ApplicationUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MakerId",
                table: "ApplicationUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecordStatus",
                table: "ApplicationUser",
                nullable: true);
        }
    }
}
