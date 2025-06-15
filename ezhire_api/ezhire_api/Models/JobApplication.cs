using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ezhire_api.Models;

public enum ApplicationStatus
{
    APPLIED,
    PENDING,
    REJECTED,
    ACCEPTED
}

[Table("job_applications")]
[Index(nameof(PostingId), nameof(ApplicantId), IsUnique = true)]
public class JobApplication : BaseEntity
{
    
    [Required]
    [Column("date_applied")]
    public DateTime DateApplied { get; set; }
    
    [Required]
    [Column("status")]
    [EnumDataType(typeof(ApplicationStatus))]
    public ApplicationStatus Status { get; set; }
    
    
    [Required]
    [Column("posting_id")]
    public int PostingId { get; set; }
    
    [Required]
    [Column("applicant_id")]
    public int ApplicantId { get; set; }
    
    [ForeignKey(nameof(PostingId))]
    public virtual JobPosting Posting { get; set; } = null!;
    
    [ForeignKey(nameof(ApplicantId))]
    public virtual Candidate Applicant { get; set; } = null!;
}