using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NETFinalProject.Migrations
{
    /// <inheritdoc />
    public partial class removedNavigationProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserExpenses_Users_UserId",
                table: "UserExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserIncomes_Users_UserId",
                table: "UserIncomes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserIncomes",
                table: "UserIncomes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserExpenses",
                table: "UserExpenses");

            migrationBuilder.RenameTable(
                name: "UserIncomes",
                newName: "Incomes");

            migrationBuilder.RenameTable(
                name: "UserExpenses",
                newName: "Expenses");

            migrationBuilder.RenameIndex(
                name: "IX_UserIncomes_UserId",
                table: "Incomes",
                newName: "IX_Incomes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserExpenses_UserId",
                table: "Expenses",
                newName: "IX_Expenses_UserId");

            migrationBuilder.AlterColumn<decimal>(
                name: "FinancialSummary_TotalIncome",
                table: "Users",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FinancialSummary_TotalExpense",
                table: "Users",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinancialSummary_StartDate",
                table: "Users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinancialSummary_EndDate",
                table: "Users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Incomes",
                table: "Incomes",
                column: "IncomeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses",
                column: "ExpenseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Users_UserId",
                table: "Expenses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Incomes_Users_UserId",
                table: "Incomes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users_UserId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Incomes_Users_UserId",
                table: "Incomes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Incomes",
                table: "Incomes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses");

            migrationBuilder.RenameTable(
                name: "Incomes",
                newName: "UserIncomes");

            migrationBuilder.RenameTable(
                name: "Expenses",
                newName: "UserExpenses");

            migrationBuilder.RenameIndex(
                name: "IX_Incomes_UserId",
                table: "UserIncomes",
                newName: "IX_UserIncomes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_UserId",
                table: "UserExpenses",
                newName: "IX_UserExpenses_UserId");

            migrationBuilder.AlterColumn<decimal>(
                name: "FinancialSummary_TotalIncome",
                table: "Users",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "FinancialSummary_TotalExpense",
                table: "Users",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinancialSummary_StartDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinancialSummary_EndDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserIncomes",
                table: "UserIncomes",
                column: "IncomeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserExpenses",
                table: "UserExpenses",
                column: "ExpenseId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExpenses_Users_UserId",
                table: "UserExpenses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserIncomes_Users_UserId",
                table: "UserIncomes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
