using ezhire_api.DAL;
using ezhire_api.DTO;
using ezhire_api.Models;
using Microsoft.EntityFrameworkCore;

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

        var newPosting = await data.JobPostings.Select(post => new JobPostingGetDto
        {
            Id = post.Id,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            JobName = post.JobName,
            DatePosted = post.DatePosted,
            Description = post.Description,
            Status = post.Status,
            CampaignId = post.CampaignId,
            Campaign = new PostingCampaignDto
            {
                Id = post.Campaign.Id,
                CreatedAt = post.Campaign.CreatedAt,
                UpdatedAt = post.Campaign.UpdatedAt,
                Name = post.Campaign.Name,
                Priority = post.Campaign.Priority
            }
        }).FirstAsync(post => post.Id == entry.Entity.Id, cancellation);

        return newPosting;
    }
}