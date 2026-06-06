using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NFChawk.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNFT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsForSale",
                table: "nfts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "nfts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_nfts_OwnerId",
                table: "nfts",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_nfts_users_OwnerId",
                table: "nfts",
                column: "OwnerId",
                principalTable: "users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_nfts_users_OwnerId",
                table: "nfts");

            migrationBuilder.DropIndex(
                name: "IX_nfts_OwnerId",
                table: "nfts");

            migrationBuilder.DropColumn(
                name: "IsForSale",
                table: "nfts");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "nfts");
        }
    }
}
