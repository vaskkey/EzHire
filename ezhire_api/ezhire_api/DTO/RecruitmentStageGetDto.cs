using System.ComponentModel.DataAnnotations;

namespace ezhire_api.DTO;

public class TechnicalMeetingGetDto : RecruitmentStageGetDto
{
    [Required] public string TechnologyName { get; set; } = null!;
}

public class TeamMeetingGetDto : RecruitmentStageGetDto
{
    [Required] public string TeamName { get; set; } = null!;
}

public class CultureMeetingGetDto : RecruitmentStageGetDto
{
    [Required] public ICollection<string> Values { get; set; }
}

public abstract class RecruitmentStageGetDto : BaseResponseDto
{
    [Required] public string Description { get; set; } = null!;

    [Required] public virtual CampaignPostingGetDto Posting { get; set; } = null!;
}