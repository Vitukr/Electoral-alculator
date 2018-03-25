using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ElectoralСalculator.Data.Migrations
{
    public partial class ForthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_Candidates_CandidateId",
                table: "Results");

            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropIndex(
                name: "IX_Results_CandidateId",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "CandidateId",
                table: "Results");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Results",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Party",
                table: "Results",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "Party",
                table: "Results");

            migrationBuilder.AddColumn<int>(
                name: "CandidateId",
                table: "Results",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlCe:ValueGeneration", "True"),
                    Name = table.Column<string>(nullable: false),
                    Party = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                });

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
    }
}
