using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ezhire_api.Migrations
{
    /// <inheritdoc />
    public partial class ManagersCreateCampaigns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "manager_id",
                table: "recruitment_campaigns",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_recruitment_campaigns_manager_id",
                table: "recruitment_campaigns",
                column: "manager_id");

            migrationBuilder.AddForeignKey(
                name: "FK_recruitment_campaigns_AspNetUsers_manager_id",
                table: "recruitment_campaigns",
                column: "manager_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recruitment_campaigns_AspNetUsers_manager_id",
                table: "recruitment_campaigns");

            migrationBuilder.DropIndex(
                name: "IX_recruitment_campaigns_manager_id",
                table: "recruitment_campaigns");

            migrationBuilder.DropColumn(
                name: "manager_id",
                table: "recruitment_campaigns");
        }
    }
}
