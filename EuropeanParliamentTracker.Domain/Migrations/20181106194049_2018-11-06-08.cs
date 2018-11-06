using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EuropeanParliamentTracker.Domain.Migrations
{
    public partial class _2018110608 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apa",
                table: "Countries");

            migrationBuilder.CreateTable(
                name: "PoliticalGroups",
                columns: table => new
                {
                    PoliticalGroupId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliticalGroups", x => x.PoliticalGroupId);
                });

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    VoteId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.VoteId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PoliticalGroups");

            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.AddColumn<string>(
                name: "Apa",
                table: "Countries",
                nullable: true);
        }
    }
}
