using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CloudMedics.Data.Migrations
{
    public partial class UpdatedNavPrperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_AspNetUsers_UserAccountId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_UserAccountId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_UserAccountId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_UserAccountId",
                table: "Doctors");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_UserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserAccountId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "UserAccountId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Patients",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Doctors",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_UserId",
                table: "Patients",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_UserId",
                table: "Doctors",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_AspNetUsers_UserId",
                table: "Doctors",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_UserId",
                table: "Patients",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_AspNetUsers_UserId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_UserId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_UserId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_UserId",
                table: "Doctors");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Patients",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "UserAccountId",
                table: "Patients",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Doctors",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "UserAccountId",
                table: "Doctors",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_UserId",
                table: "AspNetUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_UserAccountId",
                table: "Patients",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_UserAccountId",
                table: "Doctors",
                column: "UserAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_AspNetUsers_UserAccountId",
                table: "Doctors",
                column: "UserAccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_UserAccountId",
                table: "Patients",
                column: "UserAccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
