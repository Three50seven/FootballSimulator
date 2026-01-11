using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FootballSimulator.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamStatisticAndScheduleEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "game_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "qualification_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qualification_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ranking_scope_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ranking_scope_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "season_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_season_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "statistic_generation_method_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statistic_generation_method_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "division_rotation_schedules",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    year_cycle = table.Column<int>(type: "int", nullable: false),
                    game_type_id = table.Column<int>(type: "int", nullable: false),
                    division_id = table.Column<int>(type: "int", nullable: false),
                    oponent_division_id = table.Column<int>(type: "int", nullable: false),
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_division_rotation_schedules", x => x.id);
                    table.ForeignKey(
                        name: "FK_division_rotation_schedules_divisions_division_id",
                        column: x => x.division_id,
                        principalTable: "divisions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_division_rotation_schedules_divisions_oponent_division_id",
                        column: x => x.oponent_division_id,
                        principalTable: "divisions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_division_rotation_schedules_game_types_game_type_id",
                        column: x => x.game_type_id,
                        principalTable: "game_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "seasons",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    year = table.Column<int>(type: "int", nullable: false),
                    type_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    begin_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seasons", x => x.id);
                    table.ForeignKey(
                        name: "FK_seasons_season_types_type_id",
                        column: x => x.type_id,
                        principalTable: "season_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "season_weeks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    season_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    begin_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_season_weeks", x => x.id);
                    table.ForeignKey(
                        name: "FK_season_weeks_seasons_season_id",
                        column: x => x.season_id,
                        principalTable: "seasons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "team_post_season_statistics",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    team_id = table.Column<int>(type: "int", nullable: false),
                    season_id = table.Column<int>(type: "int", nullable: false),
                    previous_regular_sesason_id = table.Column<int>(type: "int", nullable: false),
                    previous_post_season_id = table.Column<int>(type: "int", nullable: false),
                    previous_final_league_rank = table.Column<int>(type: "int", nullable: false),
                    previous_win_percentage = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    previous_total_points_for = table.Column<int>(type: "int", nullable: false),
                    previous_total_points_against = table.Column<int>(type: "int", nullable: false),
                    previous_point_differential = table.Column<int>(type: "int", nullable: false),
                    previous_home_win_percentage = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    previous_offense_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    previous_defense_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    previous_special_teams_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    previous_coach_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    previous_overall_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    previous_home_advantage_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    playoff_games_played = table.Column<int>(type: "int", nullable: false),
                    playoff_wins = table.Column<int>(type: "int", nullable: false),
                    playoff_losses = table.Column<int>(type: "int", nullable: false),
                    upset_wins = table.Column<int>(type: "int", nullable: false),
                    playoff_points_for = table.Column<int>(type: "int", nullable: false),
                    playoff_points_against = table.Column<int>(type: "int", nullable: false),
                    draft_position = table.Column<int>(type: "int", nullable: false),
                    draft_boost_offense_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    draft_boost_defense_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    draft_variance = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    playoff_offense_bonus = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    playoff_defense_bonus = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    playoff_special_teams_bonus = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    playoff_coach_bonus = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    upset_bonus_total = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    calculated_offense_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    calculated_defense_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    calculated_special_teams_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    calculated_coach_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    calculated_overall_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    home_advantage_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    final_offense_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    final_defense_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    final_special_teams_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    final_coach_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    final_overall_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    final_home_advantage_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    statistic_generation_method_type_id = table.Column<int>(type: "int", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_team_post_season_statistics", x => x.id);
                    table.ForeignKey(
                        name: "FK_team_post_season_statistics_seasons_previous_post_season_id",
                        column: x => x.previous_post_season_id,
                        principalTable: "seasons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_team_post_season_statistics_seasons_previous_regular_sesason_id",
                        column: x => x.previous_regular_sesason_id,
                        principalTable: "seasons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_team_post_season_statistics_seasons_season_id",
                        column: x => x.season_id,
                        principalTable: "seasons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_team_post_season_statistics_statistic_generation_method_types_statistic_generation_method_type_id",
                        column: x => x.statistic_generation_method_type_id,
                        principalTable: "statistic_generation_method_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_team_post_season_statistics_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "team_ratings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    team_id = table.Column<int>(type: "int", nullable: false),
                    season_id = table.Column<int>(type: "int", nullable: false),
                    offense_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    defense_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    special_teams_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    coaching_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    overall_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    home_field_advantage_rating = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_team_ratings", x => x.id);
                    table.ForeignKey(
                        name: "FK_team_ratings_seasons_season_id",
                        column: x => x.season_id,
                        principalTable: "seasons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_team_ratings_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "team_season_statistics",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    team_id = table.Column<int>(type: "int", nullable: false),
                    season_id = table.Column<int>(type: "int", nullable: false),
                    post_season_id = table.Column<int>(type: "int", nullable: false),
                    wins = table.Column<int>(type: "int", nullable: false),
                    losses = table.Column<int>(type: "int", nullable: false),
                    ties = table.Column<int>(type: "int", nullable: false),
                    win_percentage = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    home_wins = table.Column<int>(type: "int", nullable: false),
                    home_games = table.Column<int>(type: "int", nullable: false),
                    home_win_percentage = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    conference_wins = table.Column<int>(type: "int", nullable: false),
                    conference_losses = table.Column<int>(type: "int", nullable: false),
                    conference_ties = table.Column<int>(type: "int", nullable: false),
                    conference_win_percentage = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    division_wins = table.Column<int>(type: "int", nullable: false),
                    division_losses = table.Column<int>(type: "int", nullable: false),
                    division_ties = table.Column<int>(type: "int", nullable: false),
                    division_win_percentage = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    points_for = table.Column<int>(type: "int", nullable: false),
                    points_against = table.Column<int>(type: "int", nullable: false),
                    point_differential = table.Column<int>(type: "int", nullable: false),
                    strength_of_schedule = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    strength_of_victory = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    division_rank = table.Column<decimal>(type: "decimal(6,5)", nullable: false),
                    conference_rank = table.Column<int>(type: "int", nullable: false),
                    qualification_type_id = table.Column<int>(type: "int", nullable: true),
                    conference_seeding = table.Column<int>(type: "int", nullable: false),
                    final_league_rank = table.Column<int>(type: "int", nullable: false),
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_team_season_statistics", x => x.id);
                    table.ForeignKey(
                        name: "FK_team_season_statistics_qualification_types_qualification_type_id",
                        column: x => x.qualification_type_id,
                        principalTable: "qualification_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_team_season_statistics_seasons_post_season_id",
                        column: x => x.post_season_id,
                        principalTable: "seasons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_team_season_statistics_seasons_season_id",
                        column: x => x.season_id,
                        principalTable: "seasons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_team_season_statistics_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "season_bye_weeks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    team_id = table.Column<int>(type: "int", nullable: false),
                    season_week_id = table.Column<int>(type: "int", nullable: false),
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_season_bye_weeks", x => x.id);
                    table.ForeignKey(
                        name: "FK_season_bye_weeks_season_weeks_season_week_id",
                        column: x => x.season_week_id,
                        principalTable: "season_weeks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_season_bye_weeks_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "season_schedules",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    week_id = table.Column<int>(type: "int", nullable: false),
                    home_team_id = table.Column<int>(type: "int", nullable: false),
                    away_team_id = table.Column<int>(type: "int", nullable: false),
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_season_schedules", x => x.id);
                    table.ForeignKey(
                        name: "FK_season_schedules_season_weeks_week_id",
                        column: x => x.week_id,
                        principalTable: "season_weeks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_season_schedules_teams_away_team_id",
                        column: x => x.away_team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_season_schedules_teams_home_team_id",
                        column: x => x.home_team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    season_schedule_id = table.Column<int>(type: "int", nullable: false),
                    home_team_score = table.Column<int>(type: "int", nullable: true),
                    away_team_score = table.Column<int>(type: "int", nullable: true),
                    first_quarter_home_score = table.Column<int>(type: "int", nullable: true),
                    first_quarter_away_score = table.Column<int>(type: "int", nullable: true),
                    second_quarter_home_score = table.Column<int>(type: "int", nullable: true),
                    second_quarter_away_score = table.Column<int>(type: "int", nullable: true),
                    third_quarter_home_score = table.Column<int>(type: "int", nullable: true),
                    third_quarter_away_score = table.Column<int>(type: "int", nullable: true),
                    fourth_quarter_home_score = table.Column<int>(type: "int", nullable: true),
                    fourth_quarter_away_score = table.Column<int>(type: "int", nullable: true),
                    overtime_home_score = table.Column<int>(type: "int", nullable: true),
                    overtime_away_score = table.Column<int>(type: "int", nullable: true),
                    stadium_id = table.Column<int>(type: "int", nullable: true),
                    stadium_roof_open = table.Column<bool>(type: "bit", nullable: false),
                    weather_type_id = table.Column<int>(type: "int", nullable: false),
                    temperature = table.Column<int>(type: "int", nullable: true),
                    wind_speed = table.Column<int>(type: "int", nullable: true),
                    wind_direction = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    humidity = table.Column<int>(type: "int", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_games", x => x.id);
                    table.ForeignKey(
                        name: "FK_games_season_schedules_season_schedule_id",
                        column: x => x.season_schedule_id,
                        principalTable: "season_schedules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_games_stadiums_stadium_id",
                        column: x => x.stadium_id,
                        principalTable: "stadiums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_games_weather_types_weather_type_id",
                        column: x => x.weather_type_id,
                        principalTable: "weather_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "game_types",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Intraconference" },
                    { 2, "Interconference" },
                    { 3, "17th Game" }
                });

            migrationBuilder.InsertData(
                table: "qualification_types",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Division Champion" },
                    { 2, "WildCard" }
                });

            migrationBuilder.InsertData(
                table: "ranking_scope_types",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Division" },
                    { 2, "Conference" },
                    { 3, "League" }
                });

            migrationBuilder.InsertData(
                table: "season_types",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Preseason" },
                    { 2, "Regular Season" },
                    { 3, "Playoffs" }
                });

            migrationBuilder.InsertData(
                table: "statistic_generation_method_types",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Random" },
                    { 2, "Performance-Based" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_division_rotation_schedules_division_id",
                table: "division_rotation_schedules",
                column: "division_id");

            migrationBuilder.CreateIndex(
                name: "IX_division_rotation_schedules_game_type_id",
                table: "division_rotation_schedules",
                column: "game_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_division_rotation_schedules_oponent_division_id",
                table: "division_rotation_schedules",
                column: "oponent_division_id");

            migrationBuilder.CreateIndex(
                name: "IX_games_season_schedule_id",
                table: "games",
                column: "season_schedule_id");

            migrationBuilder.CreateIndex(
                name: "IX_games_stadium_id",
                table: "games",
                column: "stadium_id");

            migrationBuilder.CreateIndex(
                name: "IX_games_weather_type_id",
                table: "games",
                column: "weather_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_season_bye_weeks_season_week_id",
                table: "season_bye_weeks",
                column: "season_week_id");

            migrationBuilder.CreateIndex(
                name: "IX_season_bye_weeks_team_id",
                table: "season_bye_weeks",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_season_schedules_away_team_id",
                table: "season_schedules",
                column: "away_team_id");

            migrationBuilder.CreateIndex(
                name: "IX_season_schedules_home_team_id",
                table: "season_schedules",
                column: "home_team_id");

            migrationBuilder.CreateIndex(
                name: "IX_season_schedules_week_id",
                table: "season_schedules",
                column: "week_id");

            migrationBuilder.CreateIndex(
                name: "IX_season_weeks_season_id",
                table: "season_weeks",
                column: "season_id");

            migrationBuilder.CreateIndex(
                name: "IX_seasons_type_id",
                table: "seasons",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_post_season_statistics_previous_post_season_id",
                table: "team_post_season_statistics",
                column: "previous_post_season_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_post_season_statistics_previous_regular_sesason_id",
                table: "team_post_season_statistics",
                column: "previous_regular_sesason_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_post_season_statistics_season_id",
                table: "team_post_season_statistics",
                column: "season_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_post_season_statistics_statistic_generation_method_type_id",
                table: "team_post_season_statistics",
                column: "statistic_generation_method_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_post_season_statistics_team_id",
                table: "team_post_season_statistics",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_ratings_season_id",
                table: "team_ratings",
                column: "season_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_ratings_team_id",
                table: "team_ratings",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_season_statistics_post_season_id",
                table: "team_season_statistics",
                column: "post_season_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_season_statistics_qualification_type_id",
                table: "team_season_statistics",
                column: "qualification_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_season_statistics_season_id",
                table: "team_season_statistics",
                column: "season_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_season_statistics_team_id",
                table: "team_season_statistics",
                column: "team_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "division_rotation_schedules");

            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropTable(
                name: "ranking_scope_types");

            migrationBuilder.DropTable(
                name: "season_bye_weeks");

            migrationBuilder.DropTable(
                name: "team_post_season_statistics");

            migrationBuilder.DropTable(
                name: "team_ratings");

            migrationBuilder.DropTable(
                name: "team_season_statistics");

            migrationBuilder.DropTable(
                name: "game_types");

            migrationBuilder.DropTable(
                name: "season_schedules");

            migrationBuilder.DropTable(
                name: "statistic_generation_method_types");

            migrationBuilder.DropTable(
                name: "qualification_types");

            migrationBuilder.DropTable(
                name: "season_weeks");

            migrationBuilder.DropTable(
                name: "seasons");

            migrationBuilder.DropTable(
                name: "season_types");
        }
    }
}
