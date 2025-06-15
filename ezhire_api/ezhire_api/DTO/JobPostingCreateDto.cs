using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ezhire_api.Models;

namespace ezhire_api.DTO;

public class CampaignPostingCreateDto
{
    [Required]
    [MaxLength(200)]
    public string JobName { get; set; } = null!;

    [Required]
    [MaxLength(1500)]
    public string Description { get; set; } = null!;
}

public class JobPostingCreateDto : CampaignPostingCreateDto
{
    [Required]
    public DateTime DatePosted { get; set; }
    
    [Required]
    public int CampaignId { get; set; }
    
    [Required]
    [EnumDataType(typeof(PostingStatus))]
    public PostingStatus Status { get; set; }
}