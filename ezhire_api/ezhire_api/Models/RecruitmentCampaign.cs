using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ezhire_api.Models;

public enum CampaignPriority
{
    LOW,
    MEDIUM,
    HIGH
}

[Table("recruitment_campaigns")]
public class RecruitmentCampaign : BaseEntity
{
    [Required]
    [Column("name")]
    [MaxLength(150)]
    public string Name { get; set; } = null!;
    
    [Required]
    [Column("priority")]
    public CampaignPriority Priority { get; set; }
}