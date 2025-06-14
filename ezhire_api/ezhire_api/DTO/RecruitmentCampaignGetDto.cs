using ezhire_api.Models;

namespace ezhire_api.DTO;

public class RecruitmentCampaignGetDto : BaseResponseDto
{
    public string Name { get; set; } = null!;
    
    public CampaignPriority Priority { get; set; }
}