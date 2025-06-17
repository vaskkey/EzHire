using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ezhire_api.Models;

[Table("offer")]
public class Offer : BaseEntity
{
   [Required] [Column("date_extended")] public DateTime DateExtended { get; set; }
   [Required] [Column("accepted")] public bool Accepted { get; set; }

   [Column("candidate_id")]
   public int CandidateId { get; set; }

   public virtual Candidate Candidate { get; set; } = null!;
}