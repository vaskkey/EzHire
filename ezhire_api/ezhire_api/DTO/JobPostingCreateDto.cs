using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ezhire_api.Models;

namespace ezhire_api.DTO;

public class JobPostingCreateDto
{
    [Required]
    [MaxLength(200)]
    public string JobName { get; set; } = null!;
   
    [Required]
    public DateTime DatePosted { get; set; }

    [Required]
    [MaxLength(1500)]
    public string Description { get; set; } = null!;
   
    [Required]
    [EnumDataType(typeof(PostingStatus))]
    public PostingStatus Status { get; set; }
   
   
    [Column("campaign_id")]
    public int CampaignId { get; set; }

    [ForeignKey(nameof(CampaignId))]
    public virtual RecruitmentCampaign Campaign { get; set; } = null!;
}