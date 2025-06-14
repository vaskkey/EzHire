using ezhire_api.DAL;
using ezhire_api.DTO;
using Microsoft.EntityFrameworkCore;

namespace ezhire_api.Repositories;

public interface ICandidateRepository
{
    public Task<ICollection<CandidateGetDto>> GetAll(CancellationToken cancellation);
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
}