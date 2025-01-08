using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class RenameExpenseDebitIdToTransactionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_splits_expense_transaction_id",
                table: "splits");

            migrationBuilder.RenameColumn(
                name: "transaction_id",
                table: "splits",
                newName: "ExpenseDebitId");

            migrationBuilder.RenameIndex(
                name: "IX_splits_transaction_id",
                table: "splits",
                newName: "IX_splits_ExpenseDebitId");

            migrationBuilder.AddForeignKey(
                name: "FK_splits_expense_ExpenseDebitId",
                table: "splits",
                column: "ExpenseDebitId",
                principalTable: "expense",
                principalColumn: "debit_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_splits_expense_ExpenseDebitId",
                table: "splits");

            migrationBuilder.RenameColumn(
                name: "ExpenseDebitId",
                table: "splits",
                newName: "transaction_id");

            migrationBuilder.RenameIndex(
                name: "IX_splits_ExpenseDebitId",
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
    }
}
