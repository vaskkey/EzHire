using System.ComponentModel.DataAnnotations;

namespace ezhire_api.DTO;

public class CandidateExperienceGetDto : BaseResponseDto
{
    [Required] public string CompanyName { get; set; } = null!;

    [Required] public string JobName { get; set; } = null!;

    [Required] public DateTime DateStarted { get; set; }

    public DateTime? DateFinished { get; set; }
}

public class CandidateGetDto : BaseResponseDto
{
    [Required] public string FirstName { get; set; } = null!;
    [Required] public string LastName { get; set; } = null!;

    [Required] [EmailAddress] public string Email { get; set; } = null!;

    [Required] public ICollection<CandidateExperienceGetDto> Experiences { get; set; } = null!;
}