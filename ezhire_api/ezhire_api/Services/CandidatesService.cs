using ezhire_api.DTO;
using ezhire_api.Exceptions;
using ezhire_api.Repositories;

namespace ezhire_api.Services;

/// <summary>
/// Defines operations for managing candidate records and extending offers.
/// </summary>
public interface ICandidatesService
{
    /// <summary>
    /// Retrieves all candidates in the system.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <returns>A collection of candidate DTOs.</returns>
    Task<ICollection<CandidateGetDto>> GetAll(CancellationToken cancellation);

    /// <summary>
    /// Retrieves a candidate by their unique identifier.
    /// Throws NotFound if the candidate does not exist.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="id">The candidate's unique identifier.</param>
    /// <returns>The candidate DTO.</returns>
    Task<CandidateGetDto> GetById(CancellationToken cancellation, int id);

    /// <summary>
    /// Creates a new candidate record.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="candidate">The candidate creation data.</param>
    /// <returns>The created candidate DTO.</returns>
    Task<CandidateGetDto> Create(CancellationToken cancellation, CandidateCreateDto candidate);

    /// <summary>
    /// Extends a job offer to the specified candidate.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="candidate">The candidate to whom the offer is extended.</param>
    /// <returns>The offer DTO representing the extended offer.</returns>
    Task<OfferGetDto> ExtendOffer(CancellationToken cancellation, CandidateGetDto candidate);
}

public class CandidatesService(ICandidateRepository data) : ICandidatesService
{
    /// <inheritdoc />
    public async Task<ICollection<CandidateGetDto>> GetAll(CancellationToken cancellation)
    {
        return await data.GetAll(cancellation);
    }

    /// <inheritdoc />
    public async Task<CandidateGetDto> GetById(CancellationToken cancellation, int id)
    {
        var candidate = await data.GetById(cancellation, id);

        if (candidate == null) throw new NotFound();

        return candidate;
    }

    /// <inheritdoc />
    public async Task<CandidateGetDto> Create(CancellationToken cancellation, CandidateCreateDto candidate)
    {
        return await data.Create(cancellation, candidate);
    }

    /// <inheritdoc />
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