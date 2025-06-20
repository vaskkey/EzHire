using System.ComponentModel.DataAnnotations;

namespace ezhire_api.DTO;

public class TechnicalMeetingCreateDto : MeetingBase
{
    [Required] [MaxLength(100)] public string TechnologyName { get; set; } = null!;
}

public class TeamMeetingCreateDto : MeetingBase
{
    [Required] [MaxLength(100)] public string TeamName { get; set; } = null!;
}

public class CultureMeetingCreateDto : MeetingBase
{
    [Required] [MinLength(1)] public ICollection<string> Values { get; set; }
}

public class GenericRecruitmentStageCreateDto : MeetingBase
{
    // Technical meeting
    public string? TechnologyName { get; set; } = null!;

    // Team meeting
    public string? TeamName { get; set; } = null!;

    // Culture Meeting
    [MinLength(1)] public ICollection<string> Values { get; set; }
}

public abstract class MeetingBase
{
    [Required] public string Description { get; set; } = null!;

    [Required] [Range(1, int.MaxValue)] public int PostingId { get; set; }

    public string RecruiterId { get; set; }
}