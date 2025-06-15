using ezhire_api.DAL;
using ezhire_api.DTO;
using ezhire_api.Models;

namespace ezhire_api.Repositories;

public interface IJobPostingRepository
{
    public Task<JobPostingGetDto> AddPosting(CancellationToken cancellation, JobPostingCreateDto posting);
}

public class JobPostingRepository(EzHireContext data) : IJobPostingRepository
{
    public async Task<JobPostingGetDto> AddPosting(CancellationToken cancellation, JobPostingCreateDto posting)
    {
        var entry = await data.JobPostings.AddAsync(new JobPosting
        {
            JobName = posting.JobName,
            DatePosted = posting.DatePosted,
            Description = posting.Description,
            Status = posting.Status,
            CampaignId = posting.CampaignId
        }, cancellation);

        await data.SaveChangesAsync(cancellation);

        return new JobPostingGetDto
        {
            Id = entry.Entity.Id,
            CreatedAt = entry.Entity.CreatedAt,
            UpdatedAt = entry.Entity.UpdatedAt,
            JobName = entry.Entity.JobName,
            DatePosted = entry.Entity.DatePosted,
            Description = entry.Entity.Description,
            Status = entry.Entity.Status,
            CampaignId = entry.Entity.CampaignId,
            Campaign = new PostingCampaignDto
            {
                Id = entry.Entity.Campaign.Id,
                CreatedAt = entry.Entity.Campaign.CreatedAt,
                UpdatedAt = entry.Entity.Campaign.UpdatedAt,
                Name = entry.Entity.Campaign.Name,
                Priority = entry.Entity.Campaign.Priority
            }
        };
    }
}