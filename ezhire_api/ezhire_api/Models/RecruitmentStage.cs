using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ezhire_api.Models;

[Table("recruitment_stages")]
public abstract class RecruitmentStage : BaseEntity
{
    [Required]
    [Column("description")]
    [MaxLength(250)]
    public string Description { get; set; } = null!;

    [Column("posting_id")] public int PostingId { get; set; }
    [Column("recruiter_id")] public string RecruiterId { get; set; }

    [ForeignKey(nameof(PostingId))] public virtual JobPosting Posting { get; set; } = null!;
    [ForeignKey(nameof(RecruiterId))] public virtual Recruiter LedBy { get; set; } = null!;

    public virtual ICollection<RecruitmentStageMeeting> Meetings { get; set; } = null!;
}