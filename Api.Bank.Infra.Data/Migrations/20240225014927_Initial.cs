using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Bank.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "bnk");

            migrationBuilder.CreateTable(
                name: "Banks",
                schema: "bnk",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "VARCHAR", maxLength: 3, nullable: false),
                    DisplayName = table.Column<string>(type: "VARCHAR", maxLength: 25, nullable: false),
                    InterestIndex = table.Column<decimal>(type: "numeric(10,5)", precision: 10, scale: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BANK_ID", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankSlips",
                schema: "bnk",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BankId = table.Column<Guid>(type: "uuid", nullable: false),
                    PayerName = table.Column<string>(type: "VARCHAR", maxLength: 150, nullable: false),
                    PayerDocument = table.Column<string>(type: "VARCHAR", maxLength: 14, nullable: false),
                    BeneficiaryName = table.Column<string>(type: "VARCHAR", maxLength: 150, nullable: false),
                    BeneficiaryDocument = table.Column<string>(type: "VARCHAR", maxLength: 14, nullable: false),
                    Value = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Observations = table.Column<string>(type: "VARCHAR", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BANKSLIP_ID", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BANK_BANKSLIPS",
                        column: x => x.BankId,
                        principalSchema: "bnk",
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_BANK_KEY",
                schema: "bnk",
                table: "Banks",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IDX_BANKSLIP_BANK_KEY",
                schema: "bnk",
                table: "BankSlips",
                column: "BankId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankSlips",
                schema: "bnk");

            migrationBuilder.DropTable(
                name: "Banks",
                schema: "bnk");
        }
    }
}
