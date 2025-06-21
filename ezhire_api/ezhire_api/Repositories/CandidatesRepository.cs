using ezhire_api.DAL;
using ezhire_api.DTO;
using ezhire_api.Models;
using Microsoft.EntityFrameworkCore;

namespace ezhire_api.Repositories;

/// <summary>
/// Interface for accessing and managing candidate data, experience, and offers.
/// </summary>
public interface ICandidateRepository
{
    /// <summary>
    /// Retrieves all candidates in the system.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <returns>A collection of all candidate DTOs, each including their experience history.</returns>
    Task<ICollection<CandidateGetDto>> GetAll(CancellationToken cancellation);

    /// <summary>
    /// Creates a new candidate and their experiences.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="candidate">The data required for creating the candidate, including experiences.</param>
    /// <returns>The created candidate DTO, with all associated experiences.</returns>
    Task<CandidateGetDto> Create(CancellationToken cancellation, CandidateCreateDto candidate);

    /// <summary>
    /// Creates a new experience entry for an existing candidate.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="candidateId">The unique identifier of the candidate.</param>
    /// <param name="experience">The details of the candidate's experience to be added.</param>
    /// <returns>The created experience entity.</returns>
    Task<Experience> CreateExperience(CancellationToken cancellation, int candidateId,
        CandidateExperienceCreateDto experience);

    /// <summary>
    /// Retrieves a candidate by their unique identifier.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="id">The unique identifier of the candidate.</param>
    /// <returns>The candidate DTO with experiences, or null if not found.</returns>
    Task<CandidateGetDto?> GetById(CancellationToken cancellation, int id);

    /// <summary>
    /// Creates a new offer for a candidate.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="offerCreateDto">The data required to create an offer.</param>
    /// <returns>The created offer DTO.</returns>
    Task<OfferGetDto> CreateOffer(CancellationToken cancellation, OfferCreateDto offerCreateDto);
}

public class CandidatesRepository(EzHireContext data) : ICandidateRepository
{
    /// <inheritdoc />
    public async Task<ICollection<CandidateGetDto>> GetAll(CancellationToken cancellation)
    {
        return await data.Candidates.Select(candidate => new CandidateGetDto
            {
                Id = candidate.Id,
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                CreatedAt = candidate.CreatedAt,
                UpdatedAt = candidate.UpdatedAt,
                Email = candidate.Email,
                Experiences = candidate.Experiences.Select(exp => new CandidateExperienceGetDto
                {
                    Id = exp.Id,
                    CompanyName = exp.CompanyName,
                    CreatedAt = exp.CreatedAt,
                    DateFinished = exp.DateFinished,
                    DateStarted = exp.DateStarted,
                    JobName = exp.JobName,
                    UpdatedAt = exp.UpdatedAt
                }).ToList()
            })
            .ToListAsync(cancellation);
    }

    /// <inheritdoc />
    public async Task<CandidateGetDto> Create(CancellationToken cancellation, CandidateCreateDto candidate)
    {
        var entity = await data.Candidates.AddAsync(new Candidate
        {
            FirstName = candidate.FirstName,
            LastName = candidate.LastName,
            Email = candidate.Email
        }, cancellation);

        await data.SaveChangesAsync(cancellation);

        var promises = candidate.Experiences.Select(exp => CreateExperience(cancellation, entity.Entity.Id, exp));

        var experiences = await Task.WhenAll(promises);

        await data.SaveChangesAsync(cancellation);

        return new CandidateGetDto
        {
            Id = entity.Entity.Id,
            CreatedAt = entity.Entity.CreatedAt,
            UpdatedAt = entity.Entity.UpdatedAt,
            FirstName = entity.Entity.FirstName,
            LastName = entity.Entity.LastName,
            Email = entity.Entity.Email,
            Experiences = experiences.Select(exp => new CandidateExperienceGetDto
            {
                Id = exp.Id,
                CreatedAt = exp.CreatedAt,
                UpdatedAt = exp.UpdatedAt,
                CompanyName = exp.CompanyName,
                JobName = exp.JobName,
                DateStarted = exp.DateStarted,
                DateFinished = exp.DateFinished
            }).ToList()
        };
    }

    /// <inheritdoc />
    public async Task<Experience> CreateExperience(CancellationToken cancellation, int candidateId,
        CandidateExperienceCreateDto experience)
    {
        var entity = await data.Experiences.AddAsync(new Experience
        {
            CompanyName = experience.CompanyName,
            JobName = experience.JobName,
            DateStarted = experience.DateStarted,
            DateFinished = experience.DateFinished,
            CandidateId = candidateId
        }, cancellation);

        return entity.Entity;
    }

    /// <inheritdoc />
    public Task<CandidateGetDto?> GetById(CancellationToken cancellation, int id)
    {
        return data.Candidates
            .Where(candidate => candidate.Id == id)
            .Select(candidate => new CandidateGetDto
            {
                Id = candidate.Id,
                CreatedAt = candidate.CreatedAt,
                UpdatedAt = candidate.UpdatedAt,
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                Email = candidate.Email,
                Experiences = candidate.Experiences.Select(exp => new CandidateExperienceGetDto
                {
                    Id = exp.Id,
                    CreatedAt = exp.CreatedAt,
                    UpdatedAt = exp.UpdatedAt,
                    CompanyName = exp.CompanyName,
                    JobName = exp.JobName,
                    DateStarted = exp.DateStarted,
                    DateFinished = exp.DateFinished
                }).ToList()
            })
            .FirstOrDefaultAsync(cancellation);
    }

    /// <inheritdoc />
    public async Task<OfferGetDto> CreateOffer(CancellationToken cancellation, OfferCreateDto offerCreateDto)
    {
        var entity = await data.Offers.AddAsync(new Offer
        {
            DateExtended = offerCreateDto.DateExtended,
            Accepted = offerCreateDto.Accepted,
            CandidateId = offerCreateDto.CandidateId
        }, cancellation);

        await data.SaveChangesAsync(cancellation);

        return new OfferGetDto
        {
            Id = entity.Entity.Id,
            CreatedAt = entity.Entity.CreatedAt,
            UpdatedAt = entity.Entity.UpdatedAt,
            DateExtended = entity.Entity.DateExtended,
            Accepted = entity.Entity.Accepted
        };
    }
}