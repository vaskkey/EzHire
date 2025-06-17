using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ezhire_api.Models;

public class TeamMeeting : RecruitmentStage
{
    [MaxLength(100)] [Column("team_name")] public string TeamName { get; set; } = null!;
}