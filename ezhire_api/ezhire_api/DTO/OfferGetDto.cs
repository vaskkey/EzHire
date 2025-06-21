using System.ComponentModel.DataAnnotations;

namespace ezhire_api.DTO;

public class OfferGetDto : BaseResponseDto
{
    [Required] public DateTime DateExtended { get; set; }
    [Required] public bool Accepted { get; set; }
}