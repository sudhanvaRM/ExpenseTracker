using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTimestampColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "expense",
                columns: table => new
                {
                    debit_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    comment = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    category = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("expense_pkey", x => x.debit_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    salt = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "income",
                columns: table => new
                {
                    credit_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    comment = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("income_pkey", x => x.credit_id);
                    table.ForeignKey(
                        name: "fk_users",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "split",
                columns: table => new
                {
                    split_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    totalamount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    status = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("split_pkey", x => x.split_id);
                    table.ForeignKey(
                        name: "fk_users",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "user_balance",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    balance = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_balance_pkey", x => x.user_id);
                    table.ForeignKey(
                        name: "fk_users",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "split_participants",
                columns: table => new
                {
                    split_id = table.Column<int>(type: "integer", nullable: false),
                    participant_id = table.Column<int>(type: "integer", nullable: false),
                    owes = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    paid_status = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "fk_split",
                        column: x => x.split_id,
                        principalTable: "split",
                        principalColumn: "split_id");
                    table.ForeignKey(
                        name: "fk_users",
                        column: x => x.participant_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_income_user_id",
                table: "income",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_split_user_id",
                table: "split",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_split_participants_participant_id",
                table: "split_participants",
                column: "participant_id");

            migrationBuilder.CreateIndex(
                name: "IX_split_participants_split_id",
                table: "split_participants",
                column: "split_id");

            migrationBuilder.CreateIndex(
                name: "users_email_key",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "expense");

            migrationBuilder.DropTable(
                name: "income");

            migrationBuilder.DropTable(
                name: "split_participants");

            migrationBuilder.DropTable(
                name: "user_balance");

            migrationBuilder.DropTable(
                name: "split");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
