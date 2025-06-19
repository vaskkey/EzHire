using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ezhire_api.Models;

public class HiringManager : User
{
    [Column("department")]
    [Required]
    [MaxLength(150)]
    public string Department { get; set; } = null!;
}