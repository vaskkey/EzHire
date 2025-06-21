namespace ezhire_api.DTO;

public class OfferCreateDto
{
    public DateTime DateExtended { get; set; }
    public bool Accepted { get; set; }
    public int CandidateId { get; set; }
}