using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NFChawk.Migrations
{
    /// <inheritdoc />
    public partial class AddBidsAndTransactionsAndPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_nfts_NFTId",
                table: "Auctions");

            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_users_SellerId",
                table: "Auctions");

            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_users_WinnerId",
                table: "Auctions");

            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Auctions_AuctionId",
                table: "Bids");

            migrationBuilder.DropForeignKey(
                name: "FK_Bids_users_BidderId",
                table: "Bids");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bids",
                table: "Bids");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Auctions",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "IsOnAuction",
                table: "nfts");

            migrationBuilder.RenameTable(
                name: "Bids",
                newName: "bids");

            migrationBuilder.RenameTable(
                name: "Auctions",
                newName: "auctions");

            migrationBuilder.RenameIndex(
                name: "IX_Bids_BidderId",
                table: "bids",
                newName: "IX_bids_BidderId");

            migrationBuilder.RenameIndex(
                name: "IX_Bids_AuctionId",
                table: "bids",
                newName: "IX_bids_AuctionId");

            migrationBuilder.RenameIndex(
                name: "IX_Auctions_WinnerId",
                table: "auctions",
                newName: "IX_auctions_WinnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Auctions_SellerId",
                table: "auctions",
                newName: "IX_auctions_SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_Auctions_NFTId",
                table: "auctions",
                newName: "IX_auctions_NFTId");

            migrationBuilder.AddColumn<decimal>(
                name: "HeldBalance",
                table: "users",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "nfts",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "BannerImageUrl",
                table: "collections",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "auctions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_bids",
                table: "bids",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_auctions",
                table: "auctions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NFTId = table.Column<int>(type: "integer", nullable: true),
                    AuctionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_transactions_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_transactions_UserId",
                table: "transactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_auctions_nfts_NFTId",
                table: "auctions",
                column: "NFTId",
                principalTable: "nfts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_auctions_users_SellerId",
                table: "auctions",
                column: "SellerId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_auctions_users_WinnerId",
                table: "auctions",
                column: "WinnerId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_bids_auctions_AuctionId",
                table: "bids",
                column: "AuctionId",
                principalTable: "auctions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bids_users_BidderId",
                table: "bids",
                column: "BidderId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_auctions_nfts_NFTId",
                table: "auctions");

            migrationBuilder.DropForeignKey(
                name: "FK_auctions_users_SellerId",
                table: "auctions");

            migrationBuilder.DropForeignKey(
                name: "FK_auctions_users_WinnerId",
                table: "auctions");

            migrationBuilder.DropForeignKey(
                name: "FK_bids_auctions_AuctionId",
                table: "bids");

            migrationBuilder.DropForeignKey(
                name: "FK_bids_users_BidderId",
                table: "bids");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bids",
                table: "bids");

            migrationBuilder.DropPrimaryKey(
                name: "PK_auctions",
                table: "auctions");

            migrationBuilder.DropColumn(
                name: "HeldBalance",
                table: "users");

            migrationBuilder.DropColumn(
                name: "IsSystem",
                table: "users");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "auctions");

            migrationBuilder.RenameTable(
                name: "bids",
                newName: "Bids");

            migrationBuilder.RenameTable(
                name: "auctions",
                newName: "Auctions");

            migrationBuilder.RenameIndex(
                name: "IX_bids_BidderId",
                table: "Bids",
                newName: "IX_Bids_BidderId");

            migrationBuilder.RenameIndex(
                name: "IX_bids_AuctionId",
                table: "Bids",
                newName: "IX_Bids_AuctionId");

            migrationBuilder.RenameIndex(
                name: "IX_auctions_WinnerId",
                table: "Auctions",
                newName: "IX_Auctions_WinnerId");

            migrationBuilder.RenameIndex(
                name: "IX_auctions_SellerId",
                table: "Auctions",
                newName: "IX_Auctions_SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_auctions_NFTId",
                table: "Auctions",
                newName: "IX_Auctions_NFTId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "nfts",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnAuction",
                table: "nfts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "BannerImageUrl",
                table: "collections",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bids",
                table: "Bids",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Auctions",
                table: "Auctions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_nfts_NFTId",
                table: "Auctions",
                column: "NFTId",
                principalTable: "nfts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_users_SellerId",
                table: "Auctions",
                column: "SellerId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_users_WinnerId",
                table: "Auctions",
                column: "WinnerId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Auctions_AuctionId",
                table: "Bids",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_users_BidderId",
                table: "Bids",
                column: "BidderId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
