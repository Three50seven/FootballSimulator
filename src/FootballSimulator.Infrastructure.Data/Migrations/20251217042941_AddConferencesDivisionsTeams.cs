using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballSimulator.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddConferencesDivisionsTeams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "conferences",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    abbreviation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conferences", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "divisions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    conference_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_divisions", x => x.id);
                    table.ForeignKey(
                        name: "FK_divisions_conferences_conference_id",
                        column: x => x.conference_id,
                        principalTable: "conferences",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    mascot = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    founded_year = table.Column<int>(type: "int", nullable: false),
                    rank_randomizer = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    division_id = table.Column<int>(type: "int", nullable: false),
                    stadium_id = table.Column<int>(type: "int", nullable: false),
                    archive = table.Column<bool>(type: "bit", nullable: false),
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    row_version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    created_user_id = table.Column<int>(type: "int", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teams", x => x.id);
                    table.ForeignKey(
                        name: "FK_teams_divisions_division_id",
                        column: x => x.division_id,
                        principalTable: "divisions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_teams_stadiums_stadium_id",
                        column: x => x.stadium_id,
                        principalTable: "stadiums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_teams_users_created_user_id",
                        column: x => x.created_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_teams_users_updated_user_id",
                        column: x => x.updated_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_divisions_conference_id",
                table: "divisions",
                column: "conference_id");

            migrationBuilder.CreateIndex(
                name: "IX_teams_created_user_id",
                table: "teams",
                column: "created_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_teams_division_id",
                table: "teams",
                column: "division_id");

            migrationBuilder.CreateIndex(
                name: "IX_teams_stadium_id",
                table: "teams",
                column: "stadium_id");

            migrationBuilder.CreateIndex(
                name: "IX_teams_updated_user_id",
                table: "teams",
                column: "updated_user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropTable(
                name: "divisions");

            migrationBuilder.DropTable(
                name: "conferences");
        }
    }
}
