using System.ComponentModel.DataAnnotations;
using ezhire_api.Models;

namespace ezhire_api.DTO;

public class ApplicantDto : BaseResponseDto
{
    [Required] public string FirstName { get; set; } = null!;
    [Required] public string LastName { get; set; } = null!;

    [Required] [EmailAddress] public string Email { get; set; } = null!;
}

public class PostingApplicationDto : BaseResponseDto
{
    [Required] public DateTime DateApplied { get; set; }

    [Required]
    [EnumDataType(typeof(ApplicationStatus))]
    public ApplicationStatus Status { get; set; }


    [Required] public int PostingId { get; set; }

    [Required] public int ApplicantId { get; set; }

    public virtual ApplicantDto Applicant { get; set; } = null!;
}

public class JobApplicationGetDto : BaseResponseDto
{
    [Required] public DateTime DateApplied { get; set; }

    [Required]
    [EnumDataType(typeof(ApplicationStatus))]
    public ApplicationStatus Status { get; set; }


    [Required] public int PostingId { get; set; }

    [Required] public int ApplicantId { get; set; }

    public virtual CampaignPostingGetDto Posting { get; set; } = null!;

    public virtual ApplicantDto Applicant { get; set; } = null!;
}