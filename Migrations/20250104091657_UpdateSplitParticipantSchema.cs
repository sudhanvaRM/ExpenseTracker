using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSplitParticipantSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SplitParticipants_splits_SplitId",
                table: "SplitParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_SplitParticipants_users_ParticipantId",
                table: "SplitParticipants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SplitParticipants",
                table: "SplitParticipants");

            migrationBuilder.RenameTable(
                name: "SplitParticipants",
                newName: "split_participants");

            migrationBuilder.RenameColumn(
                name: "ParticipantId",
                table: "split_participants",
                newName: "participant_id");

            migrationBuilder.RenameIndex(
                name: "IX_SplitParticipants_ParticipantId",
                table: "split_participants",
                newName: "IX_split_participants_participant_id");

            migrationBuilder.AlterColumn<bool>(
                name: "PaidStatus",
                table: "split_participants",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddPrimaryKey(
                name: "PK_split_participants",
                table: "split_participants",
                columns: new[] { "SplitId", "participant_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_split_participants_splits_SplitId",
                table: "split_participants",
                column: "SplitId",
                principalTable: "splits",
                principalColumn: "split_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_split_participants_users_participant_id",
                table: "split_participants",
                column: "participant_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_split_participants_splits_SplitId",
                table: "split_participants");

            migrationBuilder.DropForeignKey(
                name: "FK_split_participants_users_participant_id",
                table: "split_participants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_split_participants",
                table: "split_participants");

            migrationBuilder.RenameTable(
                name: "split_participants",
                newName: "SplitParticipants");

            migrationBuilder.RenameColumn(
                name: "participant_id",
                table: "SplitParticipants",
                newName: "ParticipantId");

            migrationBuilder.RenameIndex(
                name: "IX_split_participants_participant_id",
                table: "SplitParticipants",
                newName: "IX_SplitParticipants_ParticipantId");

            migrationBuilder.AlterColumn<bool>(
                name: "PaidStatus",
                table: "SplitParticipants",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
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
        }
    }
}
