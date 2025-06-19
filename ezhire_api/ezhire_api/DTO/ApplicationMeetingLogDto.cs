using System.ComponentModel.DataAnnotations;
using ezhire_api.Validators;

namespace ezhire_api.DTO;

public class ApplicationMeetingPlanDto
{
    [Required]
    [NotInPast]
    public DateOnly Date { get; set; }

    [Required] [Range(1, int.MaxValue)] public int RecruitmentStageId { get; set; }

    [Required] [Range(1, int.MaxValue)] public int ApplicationId { get; set; }
}

public class ApplicationMeetingLogDto
{
    [Required] [Range(1, int.MaxValue)] public int MeetingId { get; set; }

    [Required] [Range(1, 5)] public int Grade { get; set; }

    [Required] public string Comment { get; set; } = null!;
}

public class RecruitmentStageMeetingGetDto : BaseResponseDto
{
    [Required] public DateOnly Date { get; set; }

    public int? Grade { get; set; }

    public string? Comment { get; set; }

    [Required] public RecruitmentStageGetDto Stage { get; set; } = null!;

    [Required] public JobApplicationGetDto Application { get; set; } = null!;
}