using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballSimulator.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedStadiumEntityToUseFSDataEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_stadiums_cities_city_id",
                table: "stadiums");

            migrationBuilder.DropForeignKey(
                name: "FK_stadiums_climate_types_climate_type_id",
                table: "stadiums");

            migrationBuilder.DropForeignKey(
                name: "FK_stadiums_stadium_types_stadium_type_id",
                table: "stadiums");

            // Drop the existing row_version column
            migrationBuilder.DropColumn(
                name: "row_version",
                table: "stadiums");

            // Recreate it as rowversion
            migrationBuilder.AddColumn<byte[]>(
                name: "row_version",
                table: "stadiums",
                rowVersion: true,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "stadiums",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "guid",
                table: "stadiums",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<bool>(
                name: "archive",
                table: "stadiums",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "stadiums",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<int>(
                name: "created_user_id",
                table: "stadiums",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_date",
                table: "stadiums",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<int>(
                name: "updated_user_id",
                table: "stadiums",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_stadiums_created_user_id",
                table: "stadiums",
                column: "created_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_stadiums_updated_user_id",
                table: "stadiums",
                column: "updated_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_stadiums_cities_city_id",
                table: "stadiums",
                column: "city_id",
                principalTable: "cities",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_stadiums_climate_types_climate_type_id",
                table: "stadiums",
                column: "climate_type_id",
                principalTable: "climate_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_stadiums_stadium_types_stadium_type_id",
                table: "stadiums",
                column: "stadium_type_id",
                principalTable: "stadium_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_stadiums_users_created_user_id",
                table: "stadiums",
                column: "created_user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_stadiums_users_updated_user_id",
                table: "stadiums",
                column: "updated_user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_stadiums_cities_city_id",
                table: "stadiums");

            migrationBuilder.DropForeignKey(
                name: "FK_stadiums_climate_types_climate_type_id",
                table: "stadiums");

            migrationBuilder.DropForeignKey(
                name: "FK_stadiums_stadium_types_stadium_type_id",
                table: "stadiums");

            migrationBuilder.DropForeignKey(
                name: "FK_stadiums_users_created_user_id",
                table: "stadiums");

            migrationBuilder.DropForeignKey(
                name: "FK_stadiums_users_updated_user_id",
                table: "stadiums");

            migrationBuilder.DropIndex(
                name: "IX_stadiums_created_user_id",
                table: "stadiums");

            migrationBuilder.DropIndex(
                name: "IX_stadiums_updated_user_id",
                table: "stadiums");

            migrationBuilder.DropColumn(
                name: "archive",
                table: "stadiums");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "stadiums");

            migrationBuilder.DropColumn(
                name: "created_user_id",
                table: "stadiums");

            migrationBuilder.DropColumn(
                name: "updated_date",
                table: "stadiums");

            migrationBuilder.DropColumn(
                name: "updated_user_id",
                table: "stadiums");

            migrationBuilder.AlterColumn<byte[]>(
                name: "row_version",
                table: "stadiums",
                type: "varbinary(max)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "rowversion",
                oldRowVersion: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "stadiums",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<Guid>(
                name: "guid",
                table: "stadiums",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NEWID()");

            migrationBuilder.AddForeignKey(
                name: "FK_stadiums_cities_city_id",
                table: "stadiums",
                column: "city_id",
                principalTable: "cities",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_stadiums_climate_types_climate_type_id",
                table: "stadiums",
                column: "climate_type_id",
                principalTable: "climate_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_stadiums_stadium_types_stadium_type_id",
                table: "stadiums",
                column: "stadium_type_id",
                principalTable: "stadium_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
