using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContentRate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeroomstructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Content_RoomDetails_RoomId",
                table: "Content");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomModelUserModel_RoomDetails_RoomsId",
                table: "RoomModelUserModel");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "RoomDetails");

            migrationBuilder.RenameColumn(
                name: "RoomDetails_Password",
                table: "RoomDetails",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "RoomDetails_IsPrivate",
                table: "RoomDetails",
                newName: "IsPrivate");

            migrationBuilder.RenameColumn(
                name: "RoomDetails_CreatorId",
                table: "RoomDetails",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "RoomDetails_CreationTime",
                table: "RoomDetails",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "RoomDetails",
                newName: "RoomModelId");

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Content_Rooms_RoomId",
                table: "Content",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomDetails_Rooms_RoomModelId",
                table: "RoomDetails",
                column: "RoomModelId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomModelUserModel_Rooms_RoomsId",
                table: "RoomModelUserModel",
                column: "RoomsId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Content_Rooms_RoomId",
                table: "Content");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomDetails_Rooms_RoomModelId",
                table: "RoomDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomModelUserModel_Rooms_RoomsId",
                table: "RoomModelUserModel");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "RoomDetails",
                newName: "RoomDetails_Password");

            migrationBuilder.RenameColumn(
                name: "IsPrivate",
                table: "RoomDetails",
                newName: "RoomDetails_IsPrivate");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "RoomDetails",
                newName: "RoomDetails_CreatorId");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "RoomDetails",
                newName: "RoomDetails_CreationTime");

            migrationBuilder.RenameColumn(
                name: "RoomModelId",
                table: "RoomDetails",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RoomDetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Content_RoomDetails_RoomId",
                table: "Content",
                column: "RoomId",
                principalTable: "RoomDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomModelUserModel_RoomDetails_RoomsId",
                table: "RoomModelUserModel",
                column: "RoomsId",
                principalTable: "RoomDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
