using System.ComponentModel;
using ezhire_api.DAL;
using ezhire_api.DTO;
using ezhire_api.Models;
using Microsoft.EntityFrameworkCore;

namespace ezhire_api.Repositories;

public interface IJobPostingRepository
{
    public Task<JobPostingGetDto> AddPosting(CancellationToken cancellation, JobPostingCreateDto posting);
    Task<ICollection<CampaignPostingGetDto>> GetAllForId(CancellationToken cancellation, int? campaignId);
    Task<JobPostingGetDto?> GetById(CancellationToken cancellation, int postingId);
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

        var newPosting = await GetById(cancellation, entry.Entity.Id);

        if (newPosting == null)
        {
            throw new InvalidAsynchronousStateException($"Posting with id {entry.Entity.Id} not found after creation");
        }

        return newPosting;
    }

    public async Task<ICollection<CampaignPostingGetDto>> GetAllForId(CancellationToken cancellation, int? campaignId)
    {
        return await data.JobPostings
            .Where(posting => campaignId == null || posting.CampaignId == campaignId)
            .AsQueryable()
            .Select(posting => new CampaignPostingGetDto
            {
                Id = posting.Id,
                CreatedAt = posting.CreatedAt,
                UpdatedAt = posting.UpdatedAt,
                JobName = posting.JobName,
                DatePosted = posting.DatePosted,
                Description = posting.Description,
                Status = posting.Status
            })
            .ToListAsync(cancellation);
    }

    public async Task<JobPostingGetDto?> GetById(CancellationToken cancellation, int postingId)
    {
        return await data.JobPostings
            .Where(post => post.Id == postingId)
            .Select(post => new JobPostingGetDto
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
            })
            .FirstOrDefaultAsync(cancellation);
    }
}