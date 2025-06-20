using System.ComponentModel.DataAnnotations;
using ezhire_api.Models;

namespace ezhire_api.DTO;

public class UserGetDto
{
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;
    
    [Required]
    public string Email { get; set; } = null!;
    
    [EnumDataType(typeof(RecruiterType))]
    public RecruiterType Type { get; set; }
    
    [MaxLength(150)]
    public string Department { get; set; } = null!;
}