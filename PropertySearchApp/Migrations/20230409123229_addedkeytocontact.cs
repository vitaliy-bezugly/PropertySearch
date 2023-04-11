using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertySearchApp.Migrations
{
    public partial class addedkeytocontact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_AspNetUsers_UserEntityId",
                table: "Contact");

            migrationBuilder.DropIndex(
                name: "IX_Contact_UserEntityId",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "Contact");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Contact",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Contact_UserId",
                table: "Contact",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_AspNetUsers_UserId",
                table: "Contact",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_AspNetUsers_UserId",
                table: "Contact");

            migrationBuilder.DropIndex(
                name: "IX_Contact_UserId",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Contact");

            migrationBuilder.AddColumn<Guid>(
                name: "UserEntityId",
                table: "Contact",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contact_UserEntityId",
                table: "Contact",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_AspNetUsers_UserEntityId",
                table: "Contact",
                column: "UserEntityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
