using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DatabasePractice.Migrations
{
    /// <inheritdoc />
    public partial class migr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Имя = table.Column<string>(type: "text", nullable: false),
                    Фамилия = table.Column<string>(type: "text", nullable: false),
                    Датарождения = table.Column<DateOnly>(name: "Дата рождения", type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Clients_pkey", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GetAvgCheck",
                columns: table => new
                {
                    Час = table.Column<decimal>(type: "numeric", nullable: false),
                    СреднийЧек = table.Column<decimal>(name: "Средний Чек", type: "numeric", nullable: false),
                    Количество = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GetPurchasesOfBirthday",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    Имя = table.Column<string>(type: "text", nullable: false),
                    Фамилия = table.Column<string>(type: "text", nullable: false),
                    Общаясумма = table.Column<decimal>(name: "Общая сумма", type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Сумма = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    Датаивремя = table.Column<DateTime>(name: "Дата и время", type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Статус = table.Column<string>(type: "text", nullable: false, defaultValueSql: "'Не обработан'::text"),
                    Клиент = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Orders_pkey", x => x.ID);
                    table.ForeignKey(
                        name: "Клиент_fk",
                        column: x => x.Клиент,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Клиент",
                table: "Orders",
                column: "Клиент");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GetAvgCheck");

            migrationBuilder.DropTable(
                name: "GetPurchasesOfBirthday");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
