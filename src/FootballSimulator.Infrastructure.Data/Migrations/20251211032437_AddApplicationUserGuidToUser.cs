using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballSimulator.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationUserGuidToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "application_user_guid",
                table: "users",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_application_user_guid",
                table: "users",
                column: "application_user_guid");

            migrationBuilder.AddForeignKey(
                name: "FK_users_application_users_application_user_guid",
                table: "users",
                column: "application_user_guid",
                principalTable: "application_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_application_users_application_user_guid",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_application_user_guid",
                table: "users");

            migrationBuilder.DropColumn(
                name: "application_user_guid",
                table: "users");
        }
    }
}
