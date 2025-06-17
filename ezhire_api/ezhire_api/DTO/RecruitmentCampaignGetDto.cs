using System.ComponentModel.DataAnnotations;
using ezhire_api.Models;

namespace ezhire_api.DTO;

public class CampaignPostingGetDto : BaseResponseDto
{
    [Required] public string JobName { get; set; } = null!;

    [Required] public DateTime DatePosted { get; set; }

    [Required] public string Description { get; set; } = null!;

    [Required]
    [EnumDataType(typeof(PostingStatus))]
    public PostingStatus Status { get; set; }
}

public class RecruitmentCampaignGetDto : BaseResponseDto
{
    [Required] public string Name { get; set; } = null!;

    [Required]
    [EnumDataType(typeof(CampaignPriority))]
    public CampaignPriority Priority { get; set; }

    [Required] public ICollection<CampaignPostingGetDto> Postings { get; set; } = null!;
}