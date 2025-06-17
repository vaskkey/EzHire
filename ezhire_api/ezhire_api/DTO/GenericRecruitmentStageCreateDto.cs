using System.ComponentModel.DataAnnotations;

namespace ezhire_api.DTO;

public class TechnicalMeetingCreateDto
{
    [Required] public string Description { get; set; } = null!;

    [Required] public int PostingId { get; set; }

    [Required] [MaxLength(100)] public string TechnologyName { get; set; } = null!;
}

public class TeamMeetingCreateDto
{
    [Required] public string Description { get; set; } = null!;

    [Required] public int PostingId { get; set; }

    [Required] [MaxLength(100)] public string TeamName { get; set; } = null!;
}

public class CultureMeetingCreateDto
{
    [Required] public string Description { get; set; } = null!;

    [Required] public int PostingId { get; set; }

    [Required] [MinLength(1)] public ICollection<string> Values { get; set; }
}

public class GenericRecruitmentStageCreateDto
{
    [Required] public string Description { get; set; } = null!;

    [Required] public int PostingId { get; set; }

    // Technical meeting
    public string? TechnologyName { get; set; } = null!;

    // Team meeting
    public string? TeamName { get; set; } = null!;

    // Culture Meeting
    [MinLength(1)] public ICollection<string>? Values { get; set; }
}