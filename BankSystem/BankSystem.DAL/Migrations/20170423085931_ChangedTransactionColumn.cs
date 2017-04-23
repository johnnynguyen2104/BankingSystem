using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankSystem.DAL.Migrations
{
    public partial class ChangedTransactionColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionHistory_Account_DestinationAccountId",
                table: "TransactionHistory");

            migrationBuilder.RenameColumn(
                name: "DestinationAccountId",
                table: "TransactionHistory",
                newName: "InteractionAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionHistory_DestinationAccountId",
                table: "TransactionHistory",
                newName: "IX_TransactionHistory_InteractionAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionHistory_Account_InteractionAccountId",
                table: "TransactionHistory",
                column: "InteractionAccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionHistory_Account_InteractionAccountId",
                table: "TransactionHistory");

            migrationBuilder.RenameColumn(
                name: "InteractionAccountId",
                table: "TransactionHistory",
                newName: "DestinationAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionHistory_InteractionAccountId",
                table: "TransactionHistory",
                newName: "IX_TransactionHistory_DestinationAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionHistory_Account_DestinationAccountId",
                table: "TransactionHistory",
                column: "DestinationAccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
