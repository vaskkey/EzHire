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
    [MinLength(2)]
    public string Name { get; set; } = null!;

    [Column("manager_id")] public string ManagerId { get; set; }

    [Required]
    [Column("priority")]
    [EnumDataType(typeof(CampaignPriority))]
    public CampaignPriority Priority { get; set; }

    [ForeignKey(nameof(ManagerId))] public virtual HiringManager CreatedBy { get; set; } = null!;

    public virtual ICollection<JobPosting> JobPostings { get; set; } = null!;
}