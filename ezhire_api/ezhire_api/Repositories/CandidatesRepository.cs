using ezhire_api.DAL;
using ezhire_api.DTO;
using ezhire_api.Models;
using Microsoft.EntityFrameworkCore;

namespace ezhire_api.Repositories;

public interface ICandidateRepository
{
    public Task<ICollection<CandidateGetDto>> GetAll(CancellationToken cancellation);
    Task<CandidateGetDto> Create(CancellationToken cancellation, CandidateCreateDto candidate);

    Task<Experience> CreateExperience(CancellationToken cancellation, int candidateId, CandidateExperienceCreateDto experience);
}

public class CandidatesRepository(EzHireContext data) : ICandidateRepository
{
    public async Task<ICollection<CandidateGetDto>> GetAll(CancellationToken cancellation)
    {
        return await data.Candidates.Select(candidate => new CandidateGetDto
            {
                Id = candidate.Id,
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                CreatedAt = candidate.CreatedAt,
                UpdatedAt = candidate.UpdatedAt,
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

    public async Task<CandidateGetDto> Create(CancellationToken cancellation, CandidateCreateDto candidate)
    {
        var entity = await data.Candidates.AddAsync(new Candidate
        {
            FirstName = candidate.FirstName,
            LastName = candidate.LastName,
            Email = candidate.Email,
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
                DateFinished = exp.DateFinished,
            }).ToList()
        };
    }

    public async Task<Experience> CreateExperience(CancellationToken cancellation, int candidateId,
        CandidateExperienceCreateDto experience)
    {
        var entity = await data.Experiences.AddAsync(new Experience
        {
            CompanyName = experience.CompanyName,
            JobName = experience.JobName,
            DateStarted = experience.DateStarted,
            DateFinished = experience.DateFinished,
            CandidateId = candidateId,
        }, cancellation);

        return entity.Entity;
    }
}