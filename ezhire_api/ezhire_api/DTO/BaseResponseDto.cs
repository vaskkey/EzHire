namespace ezhire_api.DTO;

public abstract class BaseResponseDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}