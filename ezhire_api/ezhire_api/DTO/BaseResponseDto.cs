using System.ComponentModel.DataAnnotations;

namespace ezhire_api.DTO;

public abstract class BaseResponseDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
    [Required]
    public DateTime UpdatedAt { get; set; }
}