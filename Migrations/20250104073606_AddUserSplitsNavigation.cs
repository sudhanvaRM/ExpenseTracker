using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSplitsNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_users",
                table: "income");

            migrationBuilder.DropForeignKey(
                name: "fk_split",
                table: "split_participants");

            migrationBuilder.DropForeignKey(
                name: "fk_user",
                table: "split_participants");

            migrationBuilder.DropPrimaryKey(
                name: "split_participant_pkey",
                table: "split_participants");

            migrationBuilder.DropIndex(
                name: "IX_split_participants_split_id",
                table: "split_participants");

            migrationBuilder.RenameTable(
                name: "split_participants",
                newName: "SplitParticipants");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "SplitParticipants",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "paid_status",
                table: "SplitParticipants",
                newName: "PaidStatus");

            migrationBuilder.RenameColumn(
                name: "participant_id",
                table: "SplitParticipants",
                newName: "ParticipantId");

            migrationBuilder.RenameColumn(
                name: "split_id",
                table: "SplitParticipants",
                newName: "SplitId");

            migrationBuilder.RenameIndex(
                name: "IX_split_participants_participant_id",
                table: "SplitParticipants",
                newName: "IX_SplitParticipants_ParticipantId");

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "salt",
                table: "users",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "splits",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<decimal>(
                name: "total_amount",
                table: "splits",
                type: "numeric",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "SplitParticipants",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<bool>(
                name: "PaidStatus",
                table: "SplitParticipants",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SplitParticipants",
                table: "SplitParticipants",
                columns: new[] { "SplitId", "ParticipantId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SplitParticipants_splits_SplitId",
                table: "SplitParticipants",
                column: "SplitId",
                principalTable: "splits",
                principalColumn: "split_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SplitParticipants_users_ParticipantId",
                table: "SplitParticipants",
                column: "ParticipantId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_splits_expense_transaction_id",
                table: "splits",
                column: "transaction_id",
                principalTable: "expense",
                principalColumn: "debit_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_splits_users_user_id",
                table: "splits",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SplitParticipants_splits_SplitId",
                table: "SplitParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_SplitParticipants_users_ParticipantId",
                table: "SplitParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_splits_expense_transaction_id",
                table: "splits");

            migrationBuilder.DropForeignKey(
                name: "FK_splits_users_user_id",
                table: "splits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SplitParticipants",
                table: "SplitParticipants");

            migrationBuilder.DropColumn(
                name: "total_amount",
                table: "splits");

            migrationBuilder.RenameTable(
                name: "SplitParticipants",
                newName: "split_participants");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "split_participants",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "PaidStatus",
                table: "split_participants",
                newName: "paid_status");

            migrationBuilder.RenameColumn(
                name: "ParticipantId",
                table: "split_participants",
                newName: "participant_id");

            migrationBuilder.RenameColumn(
                name: "SplitId",
                table: "split_participants",
                newName: "split_id");

            migrationBuilder.RenameIndex(
                name: "IX_SplitParticipants_ParticipantId",
                table: "split_participants",
                newName: "IX_split_participants_participant_id");

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "salt",
                table: "users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "splits",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<decimal>(
                name: "amount",
                table: "split_participants",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<bool>(
                name: "paid_status",
                table: "split_participants",
                type: "boolean",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddPrimaryKey(
                name: "split_participant_pkey",
                table: "split_participants",
                columns: new[] { "split_id", "participant_id" });

            migrationBuilder.CreateIndex(
                name: "IX_split_participants_split_id",
                table: "split_participants",
                column: "split_id");

            migrationBuilder.AddForeignKey(
                name: "fk_users",
                table: "income",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_split",
                table: "split_participants",
                column: "split_id",
                principalTable: "splits",
                principalColumn: "split_id");

            migrationBuilder.AddForeignKey(
                name: "fk_user",
                table: "split_participants",
                column: "participant_id",
                principalTable: "users",
                principalColumn: "user_id");
        }
    }
}
