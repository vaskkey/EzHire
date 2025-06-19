using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ezhire_api.Models;

[Table("recruitment_stage_meeting")]
[Index(nameof(ApplicationId), nameof(RecruitmentStageId), IsUnique = true)]
public class RecruitmentStageMeeting : BaseEntity
{
   [Required]
   [Column("date")]
   public DateOnly Date { get; set; }

   [Column("grade")]
   public int? Grade { get; set; }
   
   [Column("comment")]
   [MaxLength(500)]
   public string? Comment { get; set; }

   [Column("application_id")]
   public int ApplicationId { get; set; }
   
   [Column("recruitment_stage_id")]
   public int RecruitmentStageId { get; set; }

   [ForeignKey(nameof(RecruitmentStageId))]
   public virtual RecruitmentStage Stage { get; set; } = null!;
   
   [ForeignKey(nameof(ApplicationId))]
   public virtual JobApplication Application { get; set; } = null!;
}