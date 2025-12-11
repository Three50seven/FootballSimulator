using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballSimulator.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenamedApplicationUserGuidToApplicationUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_AspNetUsers_application_user_guid",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "application_user_guid",
                table: "users",
                newName: "application_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_users_application_user_guid",
                table: "users",
                newName: "IX_users_application_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_users_AspNetUsers_application_user_id",
                table: "users",
                column: "application_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_AspNetUsers_application_user_id",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "application_user_id",
                table: "users",
                newName: "application_user_guid");

            migrationBuilder.RenameIndex(
                name: "IX_users_application_user_id",
                table: "users",
                newName: "IX_users_application_user_guid");

            migrationBuilder.AddForeignKey(
                name: "FK_users_AspNetUsers_application_user_guid",
                table: "users",
                column: "application_user_guid",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
