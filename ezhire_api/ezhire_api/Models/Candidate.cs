using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ezhire_api.Models;

[Table("candidates")]
public class Candidate : BaseEntity
{
    [Key] [Column("id")] public int Id { get; set; }

    [Column("first_name")]
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = null!;
}