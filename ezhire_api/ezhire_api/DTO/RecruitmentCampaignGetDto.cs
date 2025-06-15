using System.ComponentModel.DataAnnotations;
using ezhire_api.Models;

namespace ezhire_api.DTO;

public class RecruitmentCampaignGetDto : BaseResponseDto
{
    [Required]
    public string Name { get; set; } = null!;
    
    [Required]
    [EnumDataType(typeof(CampaignPriority))]
    public CampaignPriority Priority { get; set; }
}