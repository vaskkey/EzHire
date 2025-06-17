using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ezhire_api.Models;

[Table("candidates")]
public class Candidate : BaseEntity
{
    [Column("first_name")]
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = null!;

    [Column("email")]
    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    public virtual ICollection<Experience> Experiences { get; set; } = null!;

    public virtual ICollection<JobApplication> Applications { get; set; } = null!;
}