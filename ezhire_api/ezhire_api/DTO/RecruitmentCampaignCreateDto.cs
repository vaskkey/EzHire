using System.ComponentModel.DataAnnotations;
using ezhire_api.Models;

namespace ezhire_api.DTO;

public class RecruitmentCampaignCreateDto
{
    
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = null!;
    
    [Required]
    public CampaignPriority Priority { get; set; }
}