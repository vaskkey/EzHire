using System.ComponentModel.DataAnnotations;

namespace ezhire_api.DTO;
public class CandidateExperienceCreateDto : BaseResponseDto
{

    [Required]
    public string CompanyName { get; set; } = null!;

    [Required]
    public string JobName { get; set; } = null!;

    [Required]
    public DateTime DateStarted { get; set; }

    public DateTime? DateFinished { get; set; }
}

public class CandidateCreateDto : BaseResponseDto
{
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    
    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = null!;
    
    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public ICollection<CandidateExperienceCreateDto> Experiences { get; set; } = null!;
}
