using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NFChawk.Migrations
{
    /// <inheritdoc />
    public partial class AddXmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "auctions");

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "auctions",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "xmin",
                table: "auctions");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "auctions",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
