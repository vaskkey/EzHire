using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ezhire_api.Migrations
{
    /// <inheritdoc />
    public partial class MakeMeetingsUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_recruitment_stage_meeting_application_id",
                table: "recruitment_stage_meeting");

            migrationBuilder.CreateIndex(
                name: "IX_recruitment_stage_meeting_application_id_recruitment_stage_~",
                table: "recruitment_stage_meeting",
                columns: new[] { "application_id", "recruitment_stage_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_recruitment_stage_meeting_application_id_recruitment_stage_~",
                table: "recruitment_stage_meeting");

            migrationBuilder.CreateIndex(
                name: "IX_recruitment_stage_meeting_application_id",
                table: "recruitment_stage_meeting",
                column: "application_id");
        }
    }
}
