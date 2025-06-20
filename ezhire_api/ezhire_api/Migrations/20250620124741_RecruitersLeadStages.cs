using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ezhire_api.Migrations
{
    /// <inheritdoc />
    public partial class RecruitersLeadStages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "recruiter_id",
                table: "recruitment_stages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_recruitment_stages_recruiter_id",
                table: "recruitment_stages",
                column: "recruiter_id");

            migrationBuilder.AddForeignKey(
                name: "FK_recruitment_stages_AspNetUsers_recruiter_id",
                table: "recruitment_stages",
                column: "recruiter_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recruitment_stages_AspNetUsers_recruiter_id",
                table: "recruitment_stages");

            migrationBuilder.DropIndex(
                name: "IX_recruitment_stages_recruiter_id",
                table: "recruitment_stages");

            migrationBuilder.DropColumn(
                name: "recruiter_id",
                table: "recruitment_stages");
        }
    }
}
