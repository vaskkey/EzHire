using ezhire_api.DTO;
using ezhire_api.Repositories;

namespace ezhire_api.Services;

public interface ICandidatesService
{
    public Task<ICollection<CandidateGetDto>> GetAll(CancellationToken cancellation);
    public Task<CandidateGetDto> Create(CancellationToken cancellation, CandidateCreateDto candidate);
}

public class CandidatesService(ICandidateRepository data) : ICandidatesService
{
    public async Task<ICollection<CandidateGetDto>> GetAll(CancellationToken cancellation)
    {
        return await data.GetAll(cancellation);
    }

    public async Task<CandidateGetDto> Create(CancellationToken cancellation, CandidateCreateDto candidate)
    {
        return await data.Create(cancellation, candidate);
    }
}