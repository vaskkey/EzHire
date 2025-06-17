using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ezhire_api.Migrations
{
    /// <inheritdoc />
    public partial class AddRecruitmentStageMeetings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "recruitment_stage_meeting",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    grade = table.Column<int>(type: "integer", nullable: true),
                    comment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    application_id = table.Column<int>(type: "integer", nullable: false),
                    recruitment_stage_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recruitment_stage_meeting", x => x.id);
                    table.ForeignKey(
                        name: "FK_recruitment_stage_meeting_job_applications_application_id",
                        column: x => x.application_id,
                        principalTable: "job_applications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_recruitment_stage_meeting_recruitment_stages_recruitment_st~",
                        column: x => x.recruitment_stage_id,
                        principalTable: "recruitment_stages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_recruitment_stage_meeting_application_id",
                table: "recruitment_stage_meeting",
                column: "application_id");

            migrationBuilder.CreateIndex(
                name: "IX_recruitment_stage_meeting_recruitment_stage_id",
                table: "recruitment_stage_meeting",
                column: "recruitment_stage_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "recruitment_stage_meeting");
        }
    }
}
