using ezhire_api.DAL;
using ezhire_api.DTO;
using ezhire_api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ezhire_api.Services;

public interface ICandidatesService
{
    public Task<ICollection<CandidateGetDto>> GetAll(CancellationToken cancellation);
}

public class CandidatesService(ICandidateRepository data) : ICandidatesService
{
    public async Task<ICollection<CandidateGetDto>> GetAll(CancellationToken cancellation)
    {
        return await data.GetAll(cancellation);
    }
}