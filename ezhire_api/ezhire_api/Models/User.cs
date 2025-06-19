using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ezhire_api.Models;

public abstract class User : IdentityUser
{
    [Column("first_name")]
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = null!;
}