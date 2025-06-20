using System.ComponentModel.DataAnnotations;
using ezhire_api.Models;

namespace ezhire_api.DTO;

public enum UserType
{
    HIRING_MANAGER,
    RECRUITER
}

public class UserCreateDto
{
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;
    
    [Required]
    public string Password { get; set; } = null!;
    
    [EmailAddress]
    [Required]
    public string Email { get; set; } = null!;
    
    [Required]
    [EnumDataType(typeof(UserType))]
    public UserType UserType { get; set; }
    
    [EnumDataType(typeof(RecruiterType))]
    public RecruiterType Type { get; set; }
    
    [MaxLength(150)]
    public string? Department { get; set; } = null!;
}

public class UserLoginDto
{
    [EmailAddress]
    [Required]
    public string Email { get; set; } = null!;
    
    [Required]
    public string Password { get; set; } = null!;
}
