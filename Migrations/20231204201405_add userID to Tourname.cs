﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vsports.Migrations
{
    /// <inheritdoc />
    public partial class adduserIDtoTourname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Tournaments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_UserId",
                table: "Tournaments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_AspNetUsers_UserId",
                table: "Tournaments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_AspNetUsers_UserId",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_UserId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tournaments");
        }
    }
}
