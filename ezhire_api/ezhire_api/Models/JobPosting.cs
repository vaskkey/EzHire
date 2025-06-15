using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ezhire_api.Models;

public enum PostingStatus
{
    OPEN,
    CLOSED
}

[Table("job_postings")]
public class JobPosting : BaseEntity
{
   [Required]
   [Column("job_name")]
   [MaxLength(200)]
   public string JobName { get; set; } = null!;
   
   [Required]
   [Column("date_posted")]
   public DateTime DatePosted { get; set; }

   [Required]
   [Column("description")]
   [MaxLength(1500)]
   public string Description { get; set; } = null!;
   
   [Required]
   [Column("status")]
   [EnumDataType(typeof(PostingStatus))]
   public PostingStatus Status { get; set; }
   
   [Required]
   [Column("campaign_id")]
   public int CampaignId { get; set; }

   [ForeignKey(nameof(CampaignId))]
   public virtual RecruitmentCampaign Campaign { get; set; } = null!;
   
   public virtual ICollection<JobApplication> Applications { get; set; } = null!;
   
   public virtual ICollection<RecruitmentStage> Stages { get; set; } = null!;
}