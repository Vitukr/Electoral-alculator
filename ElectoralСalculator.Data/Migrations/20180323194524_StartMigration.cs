using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ElectoralСalculator.Data.Migrations
{
    public partial class StartMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlCe:ValueGeneration", "True"),
                    Name = table.Column<string>(nullable: true),
                    Party = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlCe:ValueGeneration", "True"),
                    CandidateId = table.Column<int>(nullable: false),
                    ValidVote = table.Column<bool>(nullable: false),
                    VoterId = table.Column<int>(nullable: false),
                    VotingRights = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Voters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlCe:ValueGeneration", "True"),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    Forename = table.Column<string>(nullable: false),
                    Pesel = table.Column<string>(nullable: false),
                    Surname = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voters", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Voters");
        }
    }
}
