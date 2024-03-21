using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NETFinalProject.Migrations
{
    /// <inheritdoc />
    public partial class next : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FinancialSummary_EndDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FinancialSummary_StartDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "FinancialSummary_TotalExpense",
                table: "Users",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FinancialSummary_TotalIncome",
                table: "Users",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinancialSummary_EndDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FinancialSummary_StartDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FinancialSummary_TotalExpense",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FinancialSummary_TotalIncome",
                table: "Users");
        }
    }
}
