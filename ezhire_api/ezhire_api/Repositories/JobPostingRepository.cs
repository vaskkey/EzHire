using System.ComponentModel;
using ezhire_api.DAL;
using ezhire_api.DTO;
using ezhire_api.Models;
using Microsoft.EntityFrameworkCore;

namespace ezhire_api.Repositories;

/// <summary>
/// Interface for managing job postings, including creation, retrieval, closing, and applicant rejection.
/// </summary>
public interface IJobPostingRepository
{
    /// <summary>
    /// Adds a new job posting.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="posting">The job posting creation data.</param>
    /// <returns>The created job posting DTO.</returns>
    public Task<JobPostingGetDto> AddPosting(CancellationToken cancellation, JobPostingCreateDto posting);

    /// <summary>
    /// Retrieves all job postings for a specific campaign, or all if campaignId is null.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="campaignId">The unique identifier of the campaign, or null to get all postings.</param>
    /// <returns>A collection of campaign posting DTOs.</returns>
    Task<ICollection<CampaignPostingGetDto>> GetAllForId(CancellationToken cancellation, int? campaignId);

    /// <summary>
    /// Retrieves a job posting by its unique identifier.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="postingId">The unique identifier of the job posting.</param>
    /// <returns>The job posting DTO, or null if not found.</returns>
    Task<JobPostingGetDto?> GetById(CancellationToken cancellation, int postingId);

    /// <summary>
    /// Closes a job posting.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="postingToClose">The job posting DTO to close.</param>
    /// <returns>The closed job posting DTO.</returns>
    Task<JobPostingGetDto> Close(CancellationToken cancellation, JobPostingGetDto postingToClose);

    /// <summary>
    /// Rejects all remaining applicants for a job posting.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="posting">The job posting DTO for which to reject applicants.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RejectRemainingApplicants(CancellationToken cancellation, JobPostingGetDto posting);
}

public class JobPostingRepository(EzHireContext data) : IJobPostingRepository
{
    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<JobPostingGetDto?> GetById(CancellationToken cancellation, int postingId)
    {
        return await data.JobPostings
            .Include(posting => posting.Campaign)
            .Include(posting => posting.Applications)
            .ThenInclude(application => application.Applicant)
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
                },
                Applications = post.Applications.Select(application => new JobApplicationGetDto
                    {
                        Id = application.Id,
                        CreatedAt = application.CreatedAt,
                        UpdatedAt = application.UpdatedAt,
                        DateApplied = application.DateApplied,
                        Status = application.Status,
                        PostingId = application.PostingId,
                        ApplicantId = application.ApplicantId,
                        Applicant = new ApplicantDto
                        {
                            Id = application.Applicant.Id,
                            CreatedAt = application.Applicant.CreatedAt,
                            UpdatedAt = application.Applicant.UpdatedAt,
                            FirstName = application.Applicant.FirstName,
                            LastName = application.Applicant.LastName,
                            Email = application.Applicant.Email
                        }
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellation);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task RejectRemainingApplicants(CancellationToken cancellation, JobPostingGetDto posting)
    {
        var applications = await data.JobApplications
            .Where(application => application.PostingId == posting.Id &&
                                  application.Status != ApplicationStatus.ACCEPTED &&
                                  application.Status != ApplicationStatus.REJECTED)
            .ToListAsync(cancellation);

        data.JobApplications.UpdateRange(applications);

        applications.ForEach(application => application.Status = ApplicationStatus.REJECTED);
        await data.SaveChangesAsync(cancellation);
    }
}