using ezhire_api.DAL;
using ezhire_api.DTO;
using Microsoft.EntityFrameworkCore;

namespace ezhire_api.Services;

public interface ICandidatesService
{
    public Task<ICollection<CandidateGetDto>> GetAll(CancellationToken cancellation);
}

public class CandidatesService(EzHireContext data) : ICandidatesService
{
    public async Task<ICollection<CandidateGetDto>> GetAll(CancellationToken cancellation)
    {
        return await data.Candidates.Select(candidate => new CandidateGetDto
            {
                Id = candidate.Id,
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                CreatedAt = candidate.CreatedAt,
                UpdatedAt = candidate.UpdatedAt
            })
            .ToListAsync(cancellation);
    }
}