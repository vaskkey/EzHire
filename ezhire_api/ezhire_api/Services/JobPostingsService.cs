using System.ComponentModel.DataAnnotations;
using ezhire_api.DTO;
using ezhire_api.Exceptions;
using ezhire_api.Models;
using ezhire_api.Repositories;

namespace ezhire_api.Services;

/// <summary>
/// Defines operations related to job postings, including creation, retrieval, closing,
/// and handling job applications.
/// </summary>
public interface IJobPostingsService
{
    /// <summary>
    /// Adds a new job posting to a given recruitment campaign.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="id">The campaign's unique identifier.</param>
    /// <param name="posting">Data for the posting to create.</param>
    /// <returns>The created job posting DTO.</returns>
    Task<JobPostingGetDto> AddPosting(CancellationToken cancellation, int id, CampaignPostingCreateDto posting);

    /// <summary>
    /// Retrieves all job postings, or all for a given campaign if campaignId is provided.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="campaignId">Optional campaign ID to filter postings.</param>
    /// <returns>Collection of campaign posting DTOs.</returns>
    Task<ICollection<CampaignPostingGetDto>> GetAllForId(CancellationToken cancellation, int? campaignId);

    /// <summary>
    /// Retrieves a job posting by its unique identifier.
    /// Throws NotFound if the posting does not exist.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="postingId">The posting's unique identifier.</param>
    /// <returns>The job posting DTO.</returns>
    Task<JobPostingGetDto> GetById(CancellationToken cancellation, int postingId);

    /// <summary>
    /// Applies to a job posting as a new or existing candidate.
    /// Throws if posting is closed or if duplicate application exists.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="id">The posting's unique identifier.</param>
    /// <param name="candidateApplication">Details of the candidate/application.</param>
    /// <returns>The created job application DTO.</returns>
    Task<JobApplicationGetDto> Apply(CancellationToken cancellation, int id, CandidateCreateDto candidateApplication);

    /// <summary>
    /// Closes a job posting, rejecting all remaining applicants.
    /// Throws if posting is not open.
    /// </summary>
    /// <param name="cancellation">Cancellation token for async operation.</param>
    /// <param name="postingId">The posting's unique identifier.</param>
    /// <returns>The updated (closed) job posting DTO.</returns>
    Task<JobPostingGetDto> Close(CancellationToken cancellation, int postingId);
}

public class JobPostingsService(
    IJobPostingRepository jobPosting,
    IJobApplicationsRepository jobApplications,
    ICandidatesService candidates)
    : IJobPostingsService
{
    /// <inheritdoc/>
    public async Task<JobPostingGetDto> AddPosting(CancellationToken cancellation, int id,
        CampaignPostingCreateDto posting)
    {
        return await jobPosting.AddPosting(cancellation, new JobPostingCreateDto
        {
            JobName = posting.JobName,
            Description = posting.Description,
            Status = PostingStatus.OPEN,
            DatePosted = DateTime.UtcNow,
            CampaignId = id
        });
    }

    /// <inheritdoc/>
    public async Task<ICollection<CampaignPostingGetDto>> GetAllForId(CancellationToken cancellation, int? campaignId)
    {
        return await jobPosting.GetAllForId(cancellation, campaignId);
    }

    /// <inheritdoc/>
    public async Task<JobPostingGetDto> GetById(CancellationToken cancellation, int postingId)
    {
        var posting = await jobPosting.GetById(cancellation, postingId);
        if (posting == null) throw new NotFound();

        return posting;
    }

    /// <inheritdoc/>
    public async Task<JobApplicationGetDto> Apply(CancellationToken cancellation, int id,
        CandidateCreateDto candidateApplication)
    {
        var posting = await jobPosting.GetById(cancellation, id);

        if (posting == null) throw new NotFound();

        if (posting.Status == PostingStatus.CLOSED) throw new BadRequest("Posting is in invalid state");

        var application = await jobApplications.GetByEmail(cancellation, candidateApplication.Email, posting.Id);
        if (application != null)
            throw new UnprocessableEntity(
                $"Application for this posting already exists for email {candidateApplication.Email}");


        var candidate = await candidates.Create(cancellation, candidateApplication);
        var newApplication = await jobApplications.Create(cancellation, new JobApplicationCreateDto
        {
            DateApplied = DateTime.UtcNow,
            Status = ApplicationStatus.APPLIED,
            ApplicantId = candidate.Id,
            PostingId = id
        });

        return newApplication;
    }

    /// <inheritdoc/>
    public async Task<JobPostingGetDto> Close(CancellationToken cancellation, int postingId)
    {
        var posting = await GetById(cancellation, postingId);

        if (posting.Status != PostingStatus.OPEN) throw new BadRequest("Posting is in invalid state");

        posting = await jobPosting.Close(cancellation, posting);
        await jobPosting.RejectRemainingApplicants(cancellation, posting);

        return posting;
    }
}