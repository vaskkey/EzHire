using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ezhire_api.Models;

public class TechnicalMeeting : RecruitmentStage
{
    [MaxLength(100)]
    [Column("technology_name")]
    public string TechnologyName { get; set; } = null!;
}