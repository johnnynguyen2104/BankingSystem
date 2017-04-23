using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankSystem.DAL.Migrations
{
    public partial class AddTransactionColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BalanceAtTime",
                table: "TransactionHistory",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "TransactionHistory",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalanceAtTime",
                table: "TransactionHistory");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "TransactionHistory");
        }
    }
}
