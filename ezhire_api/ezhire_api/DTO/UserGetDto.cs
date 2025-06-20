using System.ComponentModel.DataAnnotations;
using ezhire_api.Models;

namespace ezhire_api.DTO;

public class RecruiterGetDto : UserGetDto
{
    [Required]
    [EnumDataType(typeof(RecruiterType))]
    public RecruiterType Type { get; set; }

    public virtual UserType UserType { get; set; } = UserType.RECRUITER;
}

public class HiringManagerGetDto : UserGetDto
{
    [Required]
    [MaxLength(150)]
    public string Department { get; set; } = null!;
    
    public virtual UserType UserType { get; set; } = UserType.HIRING_MANAGER;
}

public class UserGetDto
{
    [Required]
    public string Id { get; set; } = null!;
    
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;
    
    [Required]
    public string Email { get; set; } = null!;
    
    [Required]
    [EnumDataType(typeof(UserType))]
    public virtual UserType UserType { get; set; }
}