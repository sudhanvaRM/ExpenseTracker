using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class SplitTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "split");

            migrationBuilder.DropColumn(
                name: "timestamp",
                table: "split");

            // migrationBuilder.RenameColumn(
            //     name: "totalamount",
            //     table: "split",
            //     newName: "total_amount");

            migrationBuilder.AddColumn<int>(
                name: "transaction_id",
                table: "split",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            // migrationBuilder.AddColumn<int>(
            //     name: "TransactionId",
            //     table: "splits",
            //     type: "integer",
            //     nullable: false,
            //     defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "splits",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.CreateIndex(
                name: "IX_split_transaction_id",
                table: "splits",
                column: "transaction_id");

            migrationBuilder.AddForeignKey(
                name: "FK_split_expense_transaction_id",
                table: "splits",
                column: "transaction_id",
                principalTable: "expense",
                principalColumn: "debit_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_split_expense_transaction_id",
                table: "split");

            migrationBuilder.DropIndex(
                name: "IX_split_transaction_id",
                table: "split");

            migrationBuilder.DropColumn(
                name: "transaction_id",
                table: "split");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "split");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "split");

            migrationBuilder.RenameColumn(
                name: "total_amount",
                table: "split",
                newName: "totalamount");

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "split",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "timestamp",
                table: "split",
                type: "timestamp without time zone",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP");
        }
    }
}
