using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EuropeanParliamentTracker.Domain.Migrations
{
    public partial class _2018110609 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VoteResults",
                columns: table => new
                {
                    VoteResultId = table.Column<Guid>(nullable: false),
                    ParliamentarianId = table.Column<Guid>(nullable: false),
                    VoteId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteResults", x => x.VoteResultId);
                    table.ForeignKey(
                        name: "FK_VoteResults_Parliamentarians_ParliamentarianId",
                        column: x => x.ParliamentarianId,
                        principalTable: "Parliamentarians",
                        principalColumn: "ParliamentarianId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VoteResults_Votes_VoteId",
                        column: x => x.VoteId,
                        principalTable: "Votes",
                        principalColumn: "VoteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VoteResults_ParliamentarianId",
                table: "VoteResults",
                column: "ParliamentarianId");

            migrationBuilder.CreateIndex(
                name: "IX_VoteResults_VoteId",
                table: "VoteResults",
                column: "VoteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoteResults");
        }
    }
}
