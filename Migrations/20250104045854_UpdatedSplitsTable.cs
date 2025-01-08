using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSplitsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_split_expense_transaction_id",
                table: "split");

            migrationBuilder.DropColumn(
                name: "total_amount",
                table: "split");

            migrationBuilder.RenameTable(
                name: "split",
                newName: "splits");

            migrationBuilder.RenameColumn(
                name: "owes",
                table: "split_participants",
                newName: "amount");

            migrationBuilder.RenameIndex(
                name: "IX_split_user_id",
                table: "splits",
                newName: "IX_splits_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_split_transaction_id",
                table: "splits",
                newName: "IX_splits_transaction_id");

            migrationBuilder.AddForeignKey(
                name: "FK_splits_expense_transaction_id",
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
                name: "FK_splits_expense_transaction_id",
                table: "splits");

            migrationBuilder.RenameTable(
                name: "splits",
                newName: "split");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "split_participants",
                newName: "owes");

            migrationBuilder.RenameIndex(
                name: "IX_splits_user_id",
                table: "split",
                newName: "IX_split_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_splits_transaction_id",
                table: "split",
                newName: "IX_split_transaction_id");

            migrationBuilder.AddColumn<decimal>(
                name: "total_amount",
                table: "split",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_split_expense_transaction_id",
                table: "split",
                column: "transaction_id",
                principalTable: "expense",
                principalColumn: "debit_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
