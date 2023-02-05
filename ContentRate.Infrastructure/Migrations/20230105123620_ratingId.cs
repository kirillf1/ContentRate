using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ContentRate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ratingId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Content_ContentModelId",
                table: "Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Ratings");

            migrationBuilder.RenameColumn(
                name: "ContentModelId",
                table: "Ratings",
                newName: "ContentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                columns: new[] { "UserId", "ContentId" });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ContentId",
                table: "Ratings",
                column: "ContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Content_ContentId",
                table: "Ratings",
                column: "ContentId",
                principalTable: "Content",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Content_ContentId",
                table: "Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_ContentId",
                table: "Ratings");

            migrationBuilder.RenameColumn(
                name: "ContentId",
                table: "Ratings",
                newName: "ContentModelId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Ratings",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                columns: new[] { "ContentModelId", "Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Content_ContentModelId",
                table: "Ratings",
                column: "ContentModelId",
                principalTable: "Content",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
