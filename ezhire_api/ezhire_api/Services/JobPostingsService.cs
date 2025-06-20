using System.ComponentModel.DataAnnotations;
using ezhire_api.DTO;
using ezhire_api.Exceptions;
using ezhire_api.Models;
using ezhire_api.Repositories;

namespace ezhire_api.Services;

public interface IJobPostingsService
{
    public Task<JobPostingGetDto> AddPosting(CancellationToken cancellation, int id,
        CampaignPostingCreateDto posting);

    Task<ICollection<CampaignPostingGetDto>> GetAllForId(CancellationToken cancellation, int? campaignId);
    Task<JobPostingGetDto> GetById(CancellationToken cancellation, int postingId);
    Task<JobApplicationGetDto> Apply(CancellationToken cancellation, int id, CandidateCreateDto candidateApplication);
    Task<JobPostingGetDto> Close(CancellationToken cancellation, int postingId);
}

public class JobPostingsService(
    IJobPostingRepository jobPosting,
    IJobApplicationsRepository jobApplications,
    ICandidatesService candidates)
    : IJobPostingsService
{
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

    public async Task<ICollection<CampaignPostingGetDto>> GetAllForId(CancellationToken cancellation, int? campaignId)
    {
        return await jobPosting.GetAllForId(cancellation, campaignId);
    }

    public async Task<JobPostingGetDto> GetById(CancellationToken cancellation, int postingId)
    {
        var posting = await jobPosting.GetById(cancellation, postingId);
        if (posting == null) throw new NotFound();

        return posting;
    }

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

    public async Task<JobPostingGetDto> Close(CancellationToken cancellation, int postingId)
    {
        var posting = await GetById(cancellation, postingId);

        if (posting.Status != PostingStatus.OPEN) throw new BadRequest("Posting is in invalid state");

        posting = await jobPosting.Close(cancellation, posting);
        await jobPosting.RejectRemainingApplicants(cancellation, posting);

        return posting;
    }
}