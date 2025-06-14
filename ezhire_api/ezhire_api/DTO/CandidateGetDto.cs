namespace ezhire_api.DTO;


public class CandidateExperienceGetDto : BaseResponseDto
{

    public string CompanyName { get; set; } = null!;

    public string JobName { get; set; } = null!;

    public DateTime DateStarted { get; set; }

    public DateTime? DateFinished { get; set; }
}

public class CandidateGetDto : BaseResponseDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public ICollection<CandidateExperienceGetDto> Experiences { get; set; } = null!;
}