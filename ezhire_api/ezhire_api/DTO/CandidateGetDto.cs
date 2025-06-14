namespace ezhire_api.DTO;

public class CandidateGetDto : BaseResponseDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}