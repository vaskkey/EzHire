using ezhire_api.DTO;
using ezhire_api.Exceptions;
using ezhire_api.Repositories;

namespace ezhire_api.Services;

public interface ICandidatesService
{
    public Task<ICollection<CandidateGetDto>> GetAll(CancellationToken cancellation);
    public Task<CandidateGetDto> GetById(CancellationToken cancellation, int id);
    public Task<CandidateGetDto> Create(CancellationToken cancellation, CandidateCreateDto candidate);
    public Task<OfferGetDto> ExtendOffer(CancellationToken cancellation, CandidateGetDto candidate);
}

public class CandidatesService(ICandidateRepository data) : ICandidatesService
{
    public async Task<ICollection<CandidateGetDto>> GetAll(CancellationToken cancellation)
    {
        return await data.GetAll(cancellation);
    }

    public async Task<CandidateGetDto> GetById(CancellationToken cancellation, int id)
    {
        var candidate = await data.GetById(cancellation, id);

        if (candidate == null) throw new NotFound();

        return candidate;
    }

    public async Task<CandidateGetDto> Create(CancellationToken cancellation, CandidateCreateDto candidate)
    {
        return await data.Create(cancellation, candidate);
    }

    public async Task<OfferGetDto> ExtendOffer(CancellationToken cancellation, CandidateGetDto candidate)
    {
        return await data.CreateOffer(cancellation, new OfferCreateDto
        {
            DateExtended = DateTime.UtcNow,
            Accepted = false,
            CandidateId = candidate.Id
        });
    }
}