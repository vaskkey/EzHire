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
    Task<JobPostingGetDto> Close(CancellationToken cancellation, JobPostingGetDto postingToClose);
    Task RejectRemainingApplicants(CancellationToken cancellation, JobPostingGetDto posting);
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

        return await GetById(cancellation, entry.Entity.Id);
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
            .Include(posting => posting.Campaign)
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

    public async Task<JobPostingGetDto> Close(CancellationToken cancellation, JobPostingGetDto postingToClose)
    {
        var posting = await data.JobPostings
            .Where(posting => posting.Id == postingToClose.Id)
            .Include(posting => posting.Campaign)
            .FirstAsync(cancellation);

        data.JobPostings.Update(posting);

        posting.Status = PostingStatus.CLOSED;

        await data.SaveChangesAsync(cancellation);

        return new JobPostingGetDto
        {
            Id = posting.Id,
            CreatedAt = posting.CreatedAt,
            UpdatedAt = posting.UpdatedAt,
            JobName = posting.JobName,
            DatePosted = posting.DatePosted,
            Description = posting.Description,
            Status = posting.Status,
            CampaignId = posting.CampaignId,
            Campaign = new PostingCampaignDto
            {
                Id = posting.Campaign.Id,
                CreatedAt = posting.Campaign.CreatedAt,
                UpdatedAt = posting.Campaign.UpdatedAt,
                Name = posting.Campaign.Name,
                Priority = posting.Campaign.Priority
            }
        };
    }

    public async Task RejectRemainingApplicants(CancellationToken cancellation, JobPostingGetDto posting)
    {
        var applications = await data.JobApplications
            .Where(application => application.PostingId == posting.Id &&
                                  application.Status != ApplicationStatus.ACCEPTED &&
                                  application.Status != ApplicationStatus.REJECTED)
            .ToListAsync(cancellation);

        data.JobApplications.UpdateRange(applications);

        applications.ForEach(application => application.Status = ApplicationStatus.REJECTED);
    }
}