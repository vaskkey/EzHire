using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ezhire_api.Models;

[Table("experiences")]
public class Experience : BaseEntity
{
   [Required]
   [Column("company_name")]
   [MaxLength(200)]
   public string CompanyName { get; set; } = null!;
   
   [Required]
   [Column("job_name")]
   [MaxLength(200)]
   public string JobName { get; set; } = null!;
   
   [Required]
   [Column("date_started")]
   public DateTime DateStarted { get; set; }
   
   [Column("date_finished")]
   public DateTime? DateFinished { get; set; }

   [Column("candidate_id")]
   public int CandidateId { get; set; }

   [ForeignKey(nameof(CandidateId))]
   public virtual Candidate Candidate { get; set; } = null!;
}