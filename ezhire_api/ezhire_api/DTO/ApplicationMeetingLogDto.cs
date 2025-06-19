using System.ComponentModel.DataAnnotations;

namespace ezhire_api.DTO;

public class ApplicationMeetingPlanDto
{
   [Required]
   public DateOnly Date { get; set; }

   [Required]
   public int RecruitmentStageId { get; set; }
   
   [Required]
   public int ApplicationId { get; set; }
}

public class ApplicationMeetingLogDto
{
   [Required]
   public int MeetingId { get; set; }
   
   [Required]
   public int Grade { get; set; }

   [Required] [MaxLength(500)] public string Comment { get; set; } = null!;
}

public class RecruitmentStageMeetingGetDto : BaseResponseDto
{
   [Required]
   public DateOnly Date { get; set; }

   public int? Grade { get; set; }
   
   public string? Comment { get; set; }

   public RecruitmentStageGetDto Stage { get; set; } = null!;
   
   public JobApplicationGetDto Application { get; set; } = null!;
}
