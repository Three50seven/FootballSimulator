using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballSimulator.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedStadiumAndClimateWeatherRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "climate_type_weather_types",
                columns: table => new
                {
                    climate_type_id = table.Column<int>(type: "int", nullable: false),
                    weather_type_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_climate_type_weather_types", x => new { x.climate_type_id, x.weather_type_id });
                    table.ForeignKey(
                        name: "FK_climate_type_weather_types_climate_types_climate_type_id",
                        column: x => x.climate_type_id,
                        principalTable: "climate_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_climate_type_weather_types_weather_types_weather_type_id",
                        column: x => x.weather_type_id,
                        principalTable: "weather_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stadiums",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    capacity = table.Column<int>(type: "int", nullable: false),
                    city_id = table.Column<int>(type: "int", nullable: false),
                    stadium_type_id = table.Column<int>(type: "int", nullable: false),
                    climate_type_id = table.Column<int>(type: "int", nullable: false),
                    is_super_bowl_candidate = table.Column<bool>(type: "bit", nullable: false),
                    is_international_match_candidate = table.Column<bool>(type: "bit", nullable: false),
                    broke_ground = table.Column<DateTime>(type: "datetime2", nullable: false),
                    opened = table.Column<DateTime>(type: "datetime2", nullable: true),
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    row_version = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stadiums", x => x.id);
                    table.ForeignKey(
                        name: "FK_stadiums_cities_city_id",
                        column: x => x.city_id,
                        principalTable: "cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stadiums_climate_types_climate_type_id",
                        column: x => x.climate_type_id,
                        principalTable: "climate_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stadiums_stadium_types_stadium_type_id",
                        column: x => x.stadium_type_id,
                        principalTable: "stadium_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_climate_type_weather_types_weather_type_id",
                table: "climate_type_weather_types",
                column: "weather_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_stadiums_city_id",
                table: "stadiums",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "IX_stadiums_climate_type_id",
                table: "stadiums",
                column: "climate_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_stadiums_stadium_type_id",
                table: "stadiums",
                column: "stadium_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "climate_type_weather_types");

            migrationBuilder.DropTable(
                name: "stadiums");
        }
    }
}
