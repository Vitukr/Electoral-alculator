using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ElectoralСalculator.Data.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Results_CandidateId",
                table: "Results",
                column: "CandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Candidates_CandidateId",
                table: "Results",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_Candidates_CandidateId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_CandidateId",
                table: "Results");
        }
    }
}
