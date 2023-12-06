using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyExchange.Migrations.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "currency",
                schema: "public",
                columns: table => new
                {
                    currency_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "public.uuid_generate_v4()"),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_currency", x => x.currency_id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "public",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "public.uuid_generate_v4()"),
                    login = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "user_account",
                schema: "public",
                columns: table => new
                {
                    account_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "public.uuid_generate_v4()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    currency_id = table.Column<Guid>(type: "uuid", nullable: false),
                    total_sum = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_account", x => x.account_id);
                    table.ForeignKey(
                        name: "FK_user_account_currency_currency_id",
                        column: x => x.currency_id,
                        principalSchema: "public",
                        principalTable: "currency",
                        principalColumn: "currency_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_account_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "public",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaction",
                schema: "public",
                columns: table => new
                {
                    transaction_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "public.uuid_generate_v4()"),
                    from_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    exchange_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    to_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    exchange_rate = table.Column<decimal>(type: "numeric", nullable: false),
                    exchange_fee = table.Column<decimal>(type: "numeric", nullable: false),
                    careated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction", x => x.transaction_id);
                    table.ForeignKey(
                        name: "FK_transaction_user_account_from_account_id",
                        column: x => x.from_account_id,
                        principalSchema: "public",
                        principalTable: "user_account",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transaction_user_account_to_account_id",
                        column: x => x.to_account_id,
                        principalSchema: "public",
                        principalTable: "user_account",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_transaction_from_account_id",
                schema: "public",
                table: "transaction",
                column: "from_account_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_to_account_id",
                schema: "public",
                table: "transaction",
                column: "to_account_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_account_currency_id",
                schema: "public",
                table: "user_account",
                column: "currency_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_account_user_id",
                schema: "public",
                table: "user_account",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transaction",
                schema: "public");

            migrationBuilder.DropTable(
                name: "user_account",
                schema: "public");

            migrationBuilder.DropTable(
                name: "currency",
                schema: "public");

            migrationBuilder.DropTable(
                name: "user",
                schema: "public");
        }
    }
}
